using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/posts")] // Modified to "api/posts" for consistency
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostsController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpPost]
    public async Task<IResult> CreateAsync(CreatePostDto postDto)
    {
        Post createdPost = await _postRepository.AddAsync(new Post
        {
            UserId = 1, // Hardcoded user ID
            Title = postDto.Title,
            Body = postDto.Body
        });
        return Results.Created($"/api/posts/{createdPost.Id}", createdPost);
    }

    [HttpGet("{id:int}")]
    public async Task<IResult> GetSingleAsync(int id)
    {
        Post? post = await _postRepository.GetSingleAsync(id);
        if (post == null) return Results.NotFound($"Post with ID {id} not found.");
        
        return Results.Ok(new PostDto { Id = post.Id, UserId = post.UserId, Title = post.Title, Body = post.Body });
    }

    [HttpGet]
    public async Task<IResult> GetManyAsync([FromQuery] int? userId)
    {
        IQueryable<Post> posts = _postRepository.GetMany();

        if (userId is not null)
        {
            posts = posts.Where(p => p.UserId == userId);
        }

        IQueryable<PostDto> postDtos =
            posts.Select(p => new PostDto { Id = p.Id, UserId = p.UserId, Title = p.Title, Body = p.Body });

        return Results.Ok(postDtos);
    }

    [HttpPut("{id:int}")]
    public async Task<IResult> UpdateAsync(int id, CreatePostDto postDto)
    {
        var postToUpdate = await _postRepository.GetSingleAsync(id);
        if (postToUpdate == null) return Results.NotFound($"Post with ID {id} not found.");

        postToUpdate.Title = postDto.Title;
        postToUpdate.Body = postDto.Body;
        postToUpdate.UserId = postDto.UserId;  // Assuming the update request may also modify the user
        
        await _postRepository.UpdateAsync(postToUpdate);
        return Results.Ok();
    }
    [HttpGet("{id:int}/comments")]
    public async Task<IResult> GetCommentsAsync(int id)
    {
        var comments = await _postRepository.GetCommentsByPostIdAsync(id);
        var commentDtos = comments.Select(c => new CommentDto
        {
            Id = c.Id,
            PostId = c.PostId,
            UserId = c.UserId,
            Body = c.Body
        });

        return Results.Ok(commentDtos);
    }
    
    [HttpPost("{postId:int}/comments")]
    public async Task<IResult> AddCommentAsync(int postId, [FromBody] CreateCommentDto commentDto)
    {
        // Retrieve the post from the repository
        var post = await _postRepository.GetSingleAsync(postId);
        if (post == null) return Results.NotFound($"Post with ID {postId} not found.");

        // Create the comment and associate it with the post
        var comment = new Comment
        {
            Id = 0, // Assuming auto-increment is managed in storage layer
            PostId = postId,
            UserId = commentDto.UserId,
            Body = commentDto.Body
        };

        // Add the comment to the post
        await _postRepository.AddCommentAsync(comment);
        return Results.Created($"/api/posts/{postId}/comments/{comment.Id}", comment);
    }



    [HttpDelete("{id:int}")]
    public async Task<IResult> DeleteAsync(int id)
    {
        var postToDelete = await _postRepository.GetSingleAsync(id);
        if (postToDelete == null) return Results.NotFound($"Post with ID {id} not found.");
        
        await _postRepository.DeleteAsync(id);
        return Results.Ok();
    }
}
