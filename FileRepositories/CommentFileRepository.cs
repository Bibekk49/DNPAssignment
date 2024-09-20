using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    public async Task<Comment> AddAsync(Comment comment)
    {
        var commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) :0;
        comment.Id = maxId + 1;
        comments.Add(comment);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
       var commenstAsJson = await File.ReadAllTextAsync(filePath);
         List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commenstAsJson)!;
            var commentToUpdate = comments.FirstOrDefault(c => c.Id == comment.Id);
            if (commentToUpdate != null)
            {
                comments.Remove(commentToUpdate);
                comments.Add(comment);
                commenstAsJson = JsonSerializer.Serialize(comments);
                await File.WriteAllTextAsync(filePath, commenstAsJson);
            }
    }

    public async Task DeleteAsync(int id)
    {
      var commenstAsJson = await File.ReadAllTextAsync(filePath);
         List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commenstAsJson)!;
            var commentToDelete = comments.SingleOrDefault(c => c.Id == id);
            if (commentToDelete != null)
            {
                comments.Remove(commentToDelete);
                commenstAsJson = JsonSerializer.Serialize(comments);
                await File.WriteAllTextAsync(filePath, commenstAsJson);
            }
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        var commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        Comment? comment = comments.SingleOrDefault(c => c.Id == id);
        return comment?? throw new InvalidOperationException("Comment not found");
    }

    public IQueryable<Comment> GetMany()
    {
        var commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments.AsQueryable()?? throw new InvalidOperationException("No comments found") ;
    }
}