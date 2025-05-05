using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class BookingRequest
    {
        public int Id { get; set; }
        public string Status { get; set; } // Pending, Confirmed, Cancelled
        public DateTime PreferredDate { get; set; }
        public DateTime ResponseDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
    }
}
