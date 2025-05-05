namespace Models.Requests;
public class PatientRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? DateOfBirth { get; set; } = null;
    public string Gender { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string MedicalHistory { get; set; }
    public string Allergies { get; set; }
}
