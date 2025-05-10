namespace Models.Requests;
public class InventoryRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; }
    public int MinimumLevel { get; set; }
    public string Supplier { get; set; }
    public DateTime ExpiryDate { get; set; }
}
