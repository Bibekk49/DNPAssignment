using System.Security.Claims;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity.Data;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private ClaimsPrincipal _currentClaimsPrincipal = new(new ClaimsIdentity());

    public SimpleAuthProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Login(string userName, string password)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/login", new LoginRequest
        {
            Password = password,
            Email = userName
        });
        
        if (!response.IsSuccessStatusCode)
            throw new Exception("Invalid Login attempt");

        var userDto = await response.Content.ReadFromJsonAsync<UserDto>();
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userDto.Name),
            new Claim("Id", userDto.Id.ToString())
            // Add more claims as needed
        };
        
        _currentClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "apiauth"));
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentClaimsPrincipal)));
    }

    public void Logout()
    {
        _currentClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentClaimsPrincipal)));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_currentClaimsPrincipal));
    }
}