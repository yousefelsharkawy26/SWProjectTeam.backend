namespace Models.Requests;
public class TreatmentPlanRequest
{
    public int DentistId { get; set; }
    public int PatientId { get; set; }
    public string TreatmentType { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public decimal Cost { get; set; }
    public string Notes { get; set; }
}
