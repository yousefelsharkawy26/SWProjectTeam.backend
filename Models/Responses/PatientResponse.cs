namespace Models.Responses;

public class PatientResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ImageUrl { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string MedicalHistory { get; set; }
    public string[] Allergies { get; set; }
    public string Status { get; set; }
    public DateTime? LastVisit { get; set; }
    public DateOnly? NextAppointment { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
}
