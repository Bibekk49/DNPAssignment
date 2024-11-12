using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> CreateAsync(CreateCommentDto commentDto);
    Task<CommentDto> GetSingleAsync(int id);
    Task<IEnumerable<CommentDto>> GetManyAsync(int? userId, int? postId);
    Task UpdateAsync(int id, CreateCommentDto commentDto);
    Task DeleteAsync(int id);
}