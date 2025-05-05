using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public class Employee
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ClinicId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;


    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    [ForeignKey(nameof(ClinicId))]
    public Clinic Clinic { get; set; }
}
