using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorApp.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }

    public async Task LoginAsync(string userName, string password)
    {
        try
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "auth/login",
                new LoginRequestDto()
                {
                    Username = userName,
                    Password = password
                });

            string content = await response.Content.ReadAsStringAsync();

            // Check if response was not successful and log status code and response content
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"Login failed: {content}, StatusCode: {response.StatusCode}";
                Console.WriteLine(errorMessage);  // This will log the error message in the console.
                throw new Exception(errorMessage);
            }

            // Deserialize user data on success
            UserDto userDto = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

            // Save user data in session storage
            string serializedData = JsonSerializer.Serialize(userDto);
            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serializedData);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userDto.Name),
                new Claim("Id", userDto.Id.ToString())
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(claimsPrincipal))
            );
        }
        catch (Exception ex)
        {
            // Log the detailed exception in the console
            Console.WriteLine($"Error during login: {ex.Message}");
            throw new Exception($"Error during login: {ex.Message}");
        }
    }



    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string userAsJson = "";
        try
        {
            userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
        }
        catch (InvalidOperationException)
        {
            return new AuthenticationState(new());
        }

        if (string.IsNullOrEmpty(userAsJson))
        {
            return new AuthenticationState(new());
        }

        UserDto userDto = JsonSerializer.Deserialize<UserDto>(userAsJson)!;
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userDto.Name),
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };
        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
        return new AuthenticationState(claimsPrincipal);
    }

    public async Task Logout()
    {
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }
}