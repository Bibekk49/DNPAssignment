using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient _client;

    public HttpCommentService(HttpClient client)
    {
        _client = client;
    }

    public async Task<CommentDto> CreateAsync(CreateCommentDto commentDto)
    {
        HttpResponseMessage httpResponse = await _client.PostAsJsonAsync("comments", commentDto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<CommentDto> GetSingleAsync(int id)
    {
        HttpResponseMessage httpResponse = await _client.GetAsync($"comments/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<IEnumerable<CommentDto>> GetManyAsync(int? userId, int? postId)
    {
        string query = string.Empty;
        if (userId.HasValue)
        {
            query += $"?userId={userId}";
        }
        if (postId.HasValue)
        {
            query += string.IsNullOrEmpty(query) ? $"?postId={postId}" : $"&postId={postId}";
        }

        HttpResponseMessage httpResponse = await _client.GetAsync($"comments{query}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<IEnumerable<CommentDto>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task UpdateAsync(int id, CreateCommentDto commentDto)
    {
        HttpResponseMessage httpResponse = await _client.PutAsJsonAsync($"comments/{id}", commentDto);
        if (!httpResponse.IsSuccessStatusCode)
        {
            string response = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(response);
        }
    }

    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage httpResponse = await _client.DeleteAsync($"comments/{id}");
        if (!httpResponse.IsSuccessStatusCode)
        {
            string response = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(response);
        }
    }
}