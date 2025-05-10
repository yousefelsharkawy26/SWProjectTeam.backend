namespace Models.Responses;
public class AppointmentPatientProfileResponse
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string Doctor { get; set; }
    public DateOnly Date { get; set; }
    public string Notes { get; set; }
    public bool Completed { get; set; }
}