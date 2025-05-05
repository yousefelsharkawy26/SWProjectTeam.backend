using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Allergic
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int PatientId { get; set; }
    [ForeignKey("PatientId")]
    public Patient Patient { get; set; }
}
