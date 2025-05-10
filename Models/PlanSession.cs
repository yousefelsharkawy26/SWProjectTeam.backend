using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public class PlanSession
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Notes { get; set; }
    public bool Completed { get; set; }

    public int TreatmentId { get; set; }
    [ForeignKey(nameof(TreatmentId))]
    public TreatmentPlan TreatmentPlan { get; set; }
}
