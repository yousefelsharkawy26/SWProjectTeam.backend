namespace Models.Requests;
public class RestockRequest
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }
}
