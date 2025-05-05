using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public partial class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; } // URL to the post image
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string UserId { get; set; } // Foreign key to User
    [ForeignKey("UserId")]
    public User User { get; set; } // Navigation property
}