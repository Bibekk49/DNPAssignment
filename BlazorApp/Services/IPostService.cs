using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDto?> AddPostAsync(CreatePostDto request);
    Task<List<PostDto>?> GetPostsAsync();
    Task<PostDto?> GetPostByIdAsync(int id);
    Task<List<CommentDto>> GetCommentsAsync(int postId);
    Task<CommentDto> AddCommentAsync(CreateCommentDto newComment);
    Task<UserDto?> GetAuthorOfPost(int postId);
}
