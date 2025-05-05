namespace Models.Requests;

public class NotificationRequest
{
    public string Type { get; set; } // Appointment, LabTest, MedicalScan, Follow
    public string Title { get; set; }
    public string Message { get; set; }
    public string UserId { get; set; }
    public string SenderId { get; set; }
}
