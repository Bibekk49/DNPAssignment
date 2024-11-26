using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCRepositories
{
    public class EFCPostRepository : IPostRepository
    {
        private readonly AppContext _context;

        public EFCPostRepository(AppContext context)
        {
            _context = context;
        }

        // Add a new post to the database
        public async Task<Post> AddAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        // Update an existing post
        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        // Delete a post by ID
        public async Task DeleteAsync(int id)
        {
            var postToDelete = await _context.Posts.FindAsync(id);
            if (postToDelete == null)
            {
                throw new KeyNotFoundException($"Post with ID '{id}' not found");
            }

            _context.Posts.Remove(postToDelete);
            await _context.SaveChangesAsync();
        }

        // Get a single post by ID
        public async Task<Post> GetSingleAsync(int id)
        {
            var post = await _context.Posts.Include(p => p.Comments)
                                           .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                throw new KeyNotFoundException($"Post with ID '{id}' not found");
            }

            return post;
        }

        // Get all posts
        public IQueryable<Post> GetMany()
        {
            return _context.Posts.Include(p => p.Comments);
        }

        // Get comments for a specific post by its ID
        public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            var post = await _context.Posts.Include(p => p.Comments)
                                           .FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found");
            }

            return post.Comments.ToList();
        }

        // Add a comment to a post
        public async Task AddCommentAsync(Comment comment)
        {
            var post = await _context.Posts.Include(p => p.Comments)
                                           .FirstOrDefaultAsync(p => p.Id == comment.PostId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found");
            }

            post.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }
    }
}
