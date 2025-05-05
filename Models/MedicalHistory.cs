using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class MedicalHistory
{
    public int Id { get; set; }
    public string MedicalRecord { get; set; }

    public int PatientId { get; set; }
    [ForeignKey(nameof(PatientId))]
    public Patient Patient { get; set; }
}
