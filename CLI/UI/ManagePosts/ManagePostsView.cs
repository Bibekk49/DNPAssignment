using Entities;

using RepositoryContracts;

namespace CLI.UI.ManagePosts
{
    public class ManagePostsView(IPostRepository postRepository)
    {
        private readonly CreatePostView _createPostView = new(postRepository);
        private readonly ListPostsView _listPostsView = new(postRepository);
        private readonly SinglePostView _singlePostView = new(postRepository);

        public async Task ShowMenuAsync()
        {
            var back = false;

            while (!back)
            {
                Console.WriteLine("\nManage Posts Menu:");
                Console.WriteLine("1. Create Post");
                Console.WriteLine("2. View List Of Posts");
                Console.WriteLine("3. View Single Post");
                Console.WriteLine("4. Update Post");
                Console.WriteLine("5. Delete Post");
                Console.WriteLine("0. Back");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await _createPostView.ShowCreatePostViewAsync();
                        break;
                    case "2":
                        await _listPostsView.ShowListPostViewAsync();
                        break;
                    case "3":
                        await _singlePostView.ViewSinglePostAsync();
                        break;
                    case "4":
                        await UpdatePostAsync();
                        break;
                    case "5":
                        await DeletePostAsync();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }

        private async Task UpdatePostAsync()
        {
            Console.Write("\nEnter post ID to update: ");
            int postId = Convert.ToInt32(Console.ReadLine());

            Post post = await postRepository.GetSingleAsync(postId);
            if (post != null)
            {
                Console.Write("Enter new title: ");
                post.Title = Console.ReadLine();

                Console.Write("Enter new body: ");
                post.Body = Console.ReadLine();

                await postRepository.UpdateAsync(post);
                Console.WriteLine("\nPost updated successfully.\n");
            }
        }

        private async Task DeletePostAsync()
        {
            Console.Write("Enter post ID to delete: ");
            int postId = Convert.ToInt32(Console.ReadLine());

            Post post = await postRepository.GetSingleAsync(postId);
            if (post != null)
            {
                await postRepository.DeleteAsync(postId);
                Console.WriteLine("Post deleted successfully.");
            }
        }
    }
}
