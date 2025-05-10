namespace Models.Responses;
public class InventoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Unit { get; set; }
    public int MinimumLevel { get; set; }
    public string Supplier { get; set; }
    public DateOnly? LastReStockedDate { get; set; }
    public int TotalQuantity { get; set; }
    public DateOnly CloselyExpiryDate { get; set; }
    public IEnumerable<StockResponse> Stocks { get; set; }
    public DateTime CreatedAt { get; set; }
}
