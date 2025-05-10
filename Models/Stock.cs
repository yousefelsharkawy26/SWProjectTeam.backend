using System.ComponentModel.DataAnnotations.Schema;

namespace Models;
public class Stock
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public DateOnly CreatedAt { get; set; }

    public int InventoryId { get; set; }
    [ForeignKey("InventoryId")]
    public Inventory Inventory { get; set; }
}
