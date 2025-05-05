using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public partial class Diagnosis
{
    public int Id { get; set; }
    public string DiagnosisCode { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int ExaminationId { get; set; }
    [ForeignKey("ExaminationId")]
    public MedicalExamination Examination { get; set; }
}