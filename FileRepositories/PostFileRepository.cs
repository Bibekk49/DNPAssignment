using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository:IPostRepository
{
    private readonly string filePath = "posts.json";
    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    public async Task<Post> AddAsync(Post post)
    {
        var postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        int maxId = posts.Count > 0 ? posts.Max(c => c.Id) :0;
        post.Id = maxId + 1;
        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        return post;
        
    }

    public async Task UpdateAsync(Post post)
    {
        var postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        var postToUpdate = posts.FirstOrDefault(c => c.Id == post.Id);
        if (postToUpdate != null)
        {
            posts.Remove(postToUpdate);
            posts.Add(post);
            postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(filePath, postsAsJson);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        var postToDelete = posts.SingleOrDefault(c => c.Id == id);
        if (postToDelete != null)
        {
            posts.Remove(postToDelete);
            postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(filePath, postsAsJson);
        }
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        var postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        Post? post = posts.SingleOrDefault(c => c.Id == id);
        return post ?? throw new Exception("Post not found");
    }

    public IQueryable<Post> GetMany()
    {
        var postsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable() ?? throw new Exception("No posts found");
    }
}