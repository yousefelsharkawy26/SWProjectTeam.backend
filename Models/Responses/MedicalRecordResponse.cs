namespace Models.Responses;

public class MedicalRecordResponse
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string Title { get; set; }
    public dynamic Type { get; set; }
    public string Description { get; set; }
    public string Doctor { get; set; }
    public DateTime CreatedAt { get; set; }
}