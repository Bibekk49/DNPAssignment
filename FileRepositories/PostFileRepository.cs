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

    public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        // Read all posts from file
        var postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        // Find the post with the specified ID
        Post? post = posts.SingleOrDefault(p => p.Id == postId);
    
        // If post not found, throw an exception
        if (post == null) throw new Exception("Post not found");

        // Return the comments associated with this post
        return post.Comments;
    }

    public async Task AddCommentAsync(Comment comment)
    {
        // Read existing posts from file
        var postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        // Find the post to add the comment to
        var post = posts.SingleOrDefault(p => p.Id == comment.PostId);
        if (post == null) throw new Exception("Post not found");

        // Check if there are any existing comments to avoid calling Max on an empty sequence
        int maxCommentId = post.Comments.Any() ? post.Comments.Max(c => c.Id) : 0;
        comment.Id = maxCommentId + 1;

        // Add the comment to the post
        post.Comments.Add(comment);

        // Save the updated posts back to the file
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }


}