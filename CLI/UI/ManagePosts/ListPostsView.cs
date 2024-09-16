using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView(IPostRepository postRepository)
{
    public Task ShowListPostViewAsync()
    {
        var queryablePost = postRepository.GetMany();

        if (!queryablePost.Any())
        {
            Console.WriteLine("There are no posts");
        }


        foreach (var post in queryablePost)
        {
            Console.WriteLine("Id: " + post.Id + " Title: " + post.Title);
        }

        return Task.CompletedTask;
    }
}