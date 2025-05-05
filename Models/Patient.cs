using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public partial class Patient
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ClinicId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    [ForeignKey(nameof(ClinicId))]
    public Clinic Clinic { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}