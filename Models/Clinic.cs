using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string ClinicPhone { get; set; }
        public string ClinicEmail { get; set; }
        public string WorkingDate { get; set; }

        public string OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }
    }
}
