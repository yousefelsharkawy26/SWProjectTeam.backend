namespace Models.Requests;
public class UpdateAppointmentStatusRequest
{
    public int AppointmentId { get; set; }
    public string AppointmentStatus { get; set; }
}
