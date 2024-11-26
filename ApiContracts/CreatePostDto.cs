namespace ApiContracts;

using System.ComponentModel.DataAnnotations;

public class CreatePostDto
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Body is required")]
    public string Body { get; set; } = string.Empty;
    public int AuthorUserId { get; set; }
}
