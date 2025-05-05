using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Notification
{
    public int Id { get; set; }
    public string Type { get; set; } // Appointment, LabTest, MedicalScan, Follow
    public string Title { get; set; } // Title of the notification
    public string Message { get; set; } // Notification message
    public bool IsRead { get; set; } = false; // Read or unread status
    public DateTime CreatedAt { get; set; } = DateTime.Now; // Date and time of notification creation
    public string UserId { get; set; } // Foreign key to Receiver
    [ForeignKey("UserId")]
    public User User { get; set; }

    public string SenderId { get; set; }
    public User Sender { get; set; }
}
