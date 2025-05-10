namespace Models.Responses;
public class TreatmentPlanResponse
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; }
    public string DentistName { get; set; }
    public string TreatmentType { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Cost { get; set; }
    public string Notes { get; set; }
    public string Status { get; set; }
    public IEnumerable<PlanSessionResponse> Sessions { get; set; }
}

