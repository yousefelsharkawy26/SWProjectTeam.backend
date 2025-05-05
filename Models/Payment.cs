using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public partial class Payment
{
    public int Id { get; set; }
    public string Type { get; set; }
    public decimal Discount { get; set; }
    public decimal Fee { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string SenderId { get; set; }
    [ForeignKey("SenderId")]
    public User Sender { get; set; }
    public string ReceiverId { get; set; }
    [ForeignKey("ReceiverId")]
    public User Receiver { get; set; }
    public int PatientId { get; set; }
    [ForeignKey("PatientId")]
    public Patient Patient { get; set; }
}