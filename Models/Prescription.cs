using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public partial class Prescription
{
    public int Id { get; set; }
    public string Instructions { get; set; }
    public string Duration { get; set; }
    public DateTime CreateAt { get; set; }
    public int? ExaminationId { get; set; }
    [ForeignKey("ExaminationId")]
    public MedicalExamination Examination { get; set; }
}