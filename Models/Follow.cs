using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public partial class Follow
{
    public int Id { get; set; }
    public string Status { get; set; } // Pending, Accepted, Rejected
    public string Type { get; set; } // Follow, Unfollow
    public DateTime? ResponseDate { get; set; }
    public DateTime? CreateAt { get; set; }
    public string senderId { get; set; }
    [ForeignKey("senderId")]
    public User Sender { get; set; }
    public string ReceiverId { get; set; }
    [ForeignKey("ReceiverId")]
    public User Receiver { get; set; }
}