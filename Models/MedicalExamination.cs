using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class MedicalExamination
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    [ForeignKey(nameof(DoctorId))]
    public Doctor Doctor { get; set; }
    public int PatientId { get; set; }
    [ForeignKey(nameof(PatientId))]
    public Patient Patient { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
