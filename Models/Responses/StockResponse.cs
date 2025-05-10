namespace Models.Responses;
public class StockResponse
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public DateOnly CreatedAt { get; set; }
}
