using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;
using System.Threading.Tasks;
using System;

namespace BlazorApp.Services
{
    public class HttpUserService : IUserService
    {
        private readonly HttpClient _client;

        public HttpUserService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<UserDto> AddUserAsync(CreateUserDto newUser)
        {
            HttpResponseMessage httpResponse = await _client.PostAsJsonAsync("api/users/create", newUser);
            string response = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to create user. Status: {httpResponse.StatusCode}, Response: {response}");
            }

            return JsonSerializer.Deserialize<UserDto>(response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task UpdateUserAsync(int id, UpdateUserDto request)
        {
            HttpResponseMessage httpResponse = await _client.PutAsJsonAsync($"api/users/{id}", request);
            if (!httpResponse.IsSuccessStatusCode)
            {
                string response = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception(response);
            }
        }

        public async Task<UserDto> GetSingleAsync(int id)
        {
            HttpResponseMessage httpResponse = await _client.GetAsync($"api/users/{id}");
            string response = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception(response);
            }

            return JsonSerializer.Deserialize<UserDto>(response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<IEnumerable<UserDto>> GetManyAsync(string? name)
        {
            string query = string.IsNullOrEmpty(name) ? "" : $"?name={name}";
            HttpResponseMessage httpResponse = await _client.GetAsync($"api/users{query}");
            string response = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception(response);
            }

            return JsonSerializer.Deserialize<IEnumerable<UserDto>>(response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task DeleteUserAsync(int id)
        {
            HttpResponseMessage httpResponse = await _client.DeleteAsync($"api/users/{id}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                string response = await httpResponse.Content.ReadAsStringAsync();
                throw new Exception(response);
            }
        }
    }
}
