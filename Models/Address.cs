using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public class Address
{
    public int Id { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}