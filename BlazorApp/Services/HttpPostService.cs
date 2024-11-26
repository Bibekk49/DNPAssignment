using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostDto?> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("api/posts", request);
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(await httpResponse.Content.ReadAsStringAsync());
        }
        return await httpResponse.Content.ReadFromJsonAsync<PostDto>()!;
    }

    public async Task<List<PostDto>?> GetPostsAsync()
    {
        return await client.GetFromJsonAsync<List<PostDto>>("api/posts")!;
    }

    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        return await client.GetFromJsonAsync<PostDto>($"api/posts/{id}")!;
    }
    public async Task<List<CommentDto>> GetCommentsAsync(int postId)
    {
        HttpResponseMessage response = await client.GetAsync($"api/posts/{postId}/comments");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrieve comments: {await response.Content.ReadAsStringAsync()}");
        }
        return await response.Content.ReadFromJsonAsync<List<CommentDto>>()!;
    }
    public async Task<CommentDto> AddCommentAsync(CreateCommentDto newComment)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync($"api/posts/{newComment.PostId}/comments", newComment);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to add comment: {await response.Content.ReadAsStringAsync()}");
        }
        return await response.Content.ReadFromJsonAsync<CommentDto>()!;
    }

    public async Task<UserDto?> GetAuthorOfPost(int postId)
    {
        HttpResponseMessage response = await client.GetAsync($"api/posts/{postId}/author");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrieve author: {await response.Content.ReadAsStringAsync()}");
        }
        return await response.Content.ReadFromJsonAsync<UserDto>()!;
    }
}
