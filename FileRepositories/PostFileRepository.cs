using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private const string FilePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        List<Post> posts = await LoadPostsAsync();
        post.Id = posts.Count > 0 ? posts.Max(p => p.Id) + 1 : 1;
        posts.Add(post);
        await SavePostsAsync(posts);
        return post;
    }


    public async Task UpdateAsync(Post post)
    {
        List<Post> posts = await LoadPostsAsync();
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new Exception($"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        await SavePostsAsync(posts);
    }

    public async Task DeleteAsync(int id)
    {
        List<Post> posts = await LoadPostsAsync();
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new Exception($"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        await SavePostsAsync(posts);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        List<Post> posts = await LoadPostsAsync();
        Post? post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new Exception($"Post with ID '{id}' not found");
        }

        return post;
    }

    public IQueryable<Post> GetMany()
    {
        return LoadPostsAsync().Result.AsQueryable();
    }

    public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        
        var postsAsJson = await File.ReadAllTextAsync(FilePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        Post? post = posts.SingleOrDefault(p => p.Id == postId);
    
        if (post == null) throw new Exception("Post not found");

        return post.Comments;
    }

    public async Task AddCommentAsync(Comment comment)
    {
        var postsAsJson = await File.ReadAllTextAsync(FilePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        var post = posts.SingleOrDefault(p => p.Id == comment.PostId);
        if (post == null) throw new Exception("Post not found");

        int maxCommentId = post.Comments.Any() ? post.Comments.Max(c => c.Id) : 0;
        comment.Id = maxCommentId + 1;

        post.Comments.Add(comment);

        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(FilePath, postsAsJson);
    }

    private static async Task<List<Post>> LoadPostsAsync()
    {
        string json = await File.ReadAllTextAsync(FilePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(json)!;
        return posts;
    }

    private static Task SavePostsAsync(List<Post> posts)
    {
        string json = JsonSerializer.Serialize(posts, new JsonSerializerOptions { WriteIndented = true });
        return File.WriteAllTextAsync(FilePath, json);
    }
}