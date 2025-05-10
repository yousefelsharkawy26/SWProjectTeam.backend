using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public partial class Doctor
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Specialization { get; set; }
    public string ProfessionalTitle { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;


    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; }
}