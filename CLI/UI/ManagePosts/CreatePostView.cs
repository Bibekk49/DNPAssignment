using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView(IPostRepository postRepository)
{
    public async Task ShowCreatePostViewAsync()
    {
        Console.Write("Enter title: ");
        var title = Console.ReadLine();

        Console.Write("Enter body: ");
        var body = Console.ReadLine();

        Console.Write("Enter userID: ");
        var userId = Convert.ToInt32(Console.ReadLine());

        if (title is null || body is null)
        {
            return;
        }

        var post = new Post { Body = body, UserId = userId, Title = title };

        await postRepository.AddAsync(post);
        Console.WriteLine($"Post with id {post.Id} is created ");
    }
}