using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public partial class Appointment
{
    public int Id { get; set; }
    public string Status { get; set; }
    public string AppointmentType { get; set; }
    public string Notes { get; set; }
    public DateTime? LastVisitDate { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;

    public string DoctorId { get; set; }
    [ForeignKey("DoctorId")]
    public virtual User Doctor { get; set; }
    public int PatientId { get; set; }
    [ForeignKey("PatientId")]
    public virtual Patient Patient { get; set; }
    public string AddedBy_Id { get; set; }
    [ForeignKey(nameof(AddedBy_Id))]
    public virtual User AddedBy { get; set; }

    public int ClinicId { get; set; }
}