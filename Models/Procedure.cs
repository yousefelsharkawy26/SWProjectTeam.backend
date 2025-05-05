using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Procedure
{
    public int Id { get; set; }
    public string Name { get; set; } // Name of the procedure
    public string Description { get; set; } // Description of the procedure
    public DateTime CreatedAt { get; set; } = DateTime.Now; // Date when the procedure was created
    public int ExaminationId { get; set; } // Foreign key to MedicalExamination
    [ForeignKey("ExaminationId")]
    public MedicalExamination Examination { get; set; } // Navigation property
}
