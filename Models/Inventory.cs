using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public class Inventory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Unit { get; set; }
    public int MinimumLevel { get; set; }
    public string Supplier { get; set; }
    public DateOnly? LastRestocked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int ClinicId { get; set; }
    [ForeignKey("ClinicId")]
    public Clinic Clinic { get; set; }
}