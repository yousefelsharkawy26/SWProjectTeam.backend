using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public class TreatmentPlan
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }

    public int DoctorId { get; set; }
    [ForeignKey(nameof(DoctorId))]
    public Doctor Doctor { get; set; }

    public int PatientId { get; set; }
    [ForeignKey(nameof(PatientId))]
    public Patient Patient { get; set; }
}
