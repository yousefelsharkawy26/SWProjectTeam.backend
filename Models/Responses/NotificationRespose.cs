namespace Models.Responses;
public class NotificationRespose
{
    public string Type { get; set; } 
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public string SenderId { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
