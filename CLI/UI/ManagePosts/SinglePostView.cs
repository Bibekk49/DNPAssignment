using Entities;

using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView(IPostRepository postRepository)
{
    public async Task ViewSinglePostAsync()
    {
        Console.Write("Enter post ID: ");
        var postId = Convert.ToInt32(Console.ReadLine());

        var  post = await postRepository.GetSingleAsync(postId);

        Console.WriteLine($"\nPost:\nID: {post.Id}\nTitle: {post.Title}\nBody: {post.Body}\n");
    }
}