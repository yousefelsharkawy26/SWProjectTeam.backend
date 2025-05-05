namespace Models.Responses;

public class AppointmentResponse
{
    public int Id { get; set; }
    public string PatientName { get; set; }
    public int PatientId { get; set; }
    public string DentistId { get; set; }
    public string DentistName { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string ImageUrl { get; set; }
    public string Type { get; set; }
    public string Notes { get; set; }
    public string Status { get; set; }
}
