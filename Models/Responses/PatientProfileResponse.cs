namespace Models.Responses;
public class PatientProfileResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public short age { get; set; }
    public string Avatar { get; set; }
    public DateOnly LastVisit { get; set; }
    public DateOnly CreatedAt { get; set; }
    public IEnumerable<AppointmentPatientProfileResponse> Appointments { get; set; }
    public IEnumerable<MedicalRecordResponse> MedicalRecords { get; set; }
}
