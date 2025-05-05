namespace Models;

public class Subscription
{
    public int Id { get; set; } // Unique identifier for the subscription
    public string Plan { get; set; } // Name of the subscription plan
    public string Description { get; set; } // Description of the subscription plan
    public decimal Price { get; set; } // Price of the subscription plan
    public bool IsActive { get; set; } = true; // Indicates if the subscription is active
    public DateTime ExpiresAt { get; set; } // Duration of the subscription
    public DateTime CreatedAt { get; set; } = DateTime.Now; // Date when the subscription was created
}
