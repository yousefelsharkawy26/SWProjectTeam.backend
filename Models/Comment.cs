using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string UserId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("PostId")]
    public Post Post { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
}
