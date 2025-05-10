using Models;
using Models.Requests;
using Models.Responses;

namespace DentalManagementSystem.Services.Interfaces;
public interface IInventoryServices
{
    IEnumerable<InventoryResponse> GetInventories(int clinicId);
    Task AddNewInventory(int clinicId, InventoryRequest inventory);
    Task Restock(int inventoryId, RestockRequest restock);
}
