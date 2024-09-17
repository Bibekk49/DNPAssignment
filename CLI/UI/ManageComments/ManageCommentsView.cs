using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ManageCommentsView(ICommentRepository commentRepository)
{
    
    public async Task ShowMenuAsync()
    {
        var back = false;

        while (!back)
        {
            Console.WriteLine("\nManage Comments Menu:");
            Console.WriteLine("1. Create Comment");
            Console.WriteLine("2. List Comments");
            Console.WriteLine("3. Update Comment");
            Console.WriteLine("4. Delete Comment");
            Console.WriteLine("0. Back");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await CreateCommentAsync();
                    break;
                case "2":
                    await ListCommentsAsync();
                    break;
                case "3":
                    await UpdateCommentAsync();
                    break;
                case "4":
                    await DeleteCommentAsync();
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
    
    private async Task CreateCommentAsync()
    {
        Console.Write("\nEnter post ID to comment: ");
        int postId = Convert.ToInt32(Console.ReadLine());
        Console.Write("\nEnter comment: ");
        string comment = Console.ReadLine();
        Console.Write("\nEnter user ID: ");
        int userId = Convert.ToInt32(Console.ReadLine());
        
        // Create comment
        Console.WriteLine("Comment created successfully");
    }
    private async Task ListCommentsAsync()
    {
        // List comments
    }
    private async Task UpdateCommentAsync()
    {
        Console.Write("\nEnter comment ID to update: ");
        int commentId = Convert.ToInt32(Console.ReadLine());
        Console.Write("\nEnter new comment: ");
        string comment = Console.ReadLine();
        
        // Update comment
        Console.WriteLine("Comment updated successfully");
    }
    private async Task DeleteCommentAsync()
    {
        Console.Write("\nEnter comment ID to delete: ");
        int commentId = Convert.ToInt32(Console.ReadLine());
        
        // Delete comment
        Console.WriteLine("Comment deleted successfully");
    }
}