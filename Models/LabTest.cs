using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public partial class LabTest
{
    public int Id { get; set; }
    public string TestName { get; set; }
    public string FileUrl { get; set; } // URL to the test result file (PDF)
    public string Results { get; set; } // Normal, Abnormal
    public string Status { get; set; } // Pending, Completed, Cancelled
    public string Notes { get; set; }
    public DateTime CreateAt { get; set; }

    public int ExaminationId { get; set; } // Foreign key to MedicalExamination
    [ForeignKey("ExaminationId")]
    public MedicalExamination Examination { get; set; } // Navigation property
}