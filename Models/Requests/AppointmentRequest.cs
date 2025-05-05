namespace Models.Requests;
public class AppointmentRequest
{
    public string PatientId { get; set; }
    public string DentistId { get; set; }
    public DateTime Date {  get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string TreatmentType { get; set; }
    public string Notes { get; set; }
}
