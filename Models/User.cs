using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public partial class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Permission { get; set; }
    public string ImageUrl { get; set; } = "avatar.png";
    public string Bio { get; set; }
    public bool IsActive { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; }

    public int? SubscriptionId { get; set; }
    [ForeignKey("SubscriptionId")]
    public virtual Subscription Subscription { get; set; }
}