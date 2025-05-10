using DataAccess.Repository.IRepository;
using DentalManagementSystem.Services.Interfaces;
using Models;
using Models.Requests;
using Models.Responses;

namespace DentalManagementSystem.Services;
public class InventoryServices : IInventoryServices
{
    private readonly IUnitOfWork _unitOfWork;
    public InventoryServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddNewInventory(int clinicId, InventoryRequest inventory)
    {
        var stock = new Stock()
        {
            Quantity = inventory.Quantity,
            ExpiryDate = new DateOnly(inventory.ExpiryDate.Year, inventory.ExpiryDate.Month, inventory.ExpiryDate.Day),
            Inventory = new Inventory()
            {
                Category = inventory.Category,
                ClinicId = clinicId,
                MinimumLevel = inventory.MinimumLevel,
                Name = inventory.Name,
                Supplier = inventory.Supplier,
                Unit = inventory.Unit,
                LastRestocked  =null,
            },
        };

        await _unitOfWork.Stock.Add(stock);
        await _unitOfWork.SaveAsync();
    }

    public IEnumerable<InventoryResponse> GetInventories(int clinicId)
    {
        var inventories = _unitOfWork.Inventory
            .GetAll(u => u.ClinicId == clinicId)
            .ToList()
            .Select(i =>
            {
                var stocks = _unitOfWork.Stock
                    .GetAll(u => u.InventoryId == i.Id)
                    .ToList()
                    .Select(s => new StockResponse()
                    {
                        Id = s.Id,
                        ExpiryDate = s.ExpiryDate,
                        Quantity = s.Quantity,
                        CreatedAt = s.CreatedAt,
                    });
                return new InventoryResponse()
                {
                    Id = i.Id,
                    Category = i.Category,
                    MinimumLevel = i.MinimumLevel,
                    Name = i.Name,
                    Supplier = i.Supplier,
                    Unit = i.Unit,
                    Stocks = stocks,
                    LastReStockedDate = i.LastRestocked,
                    TotalQuantity = stocks.Select(s => s.Quantity).Sum(),
                    CloselyExpiryDate = stocks.Select(s => s.ExpiryDate).Min(),
                    CreatedAt = i.CreatedAt,
                };
            });

        return inventories;
    }

    public async Task Restock(int inventoryId, RestockRequest restock)
    {
        var dt = DateTime.Now;

        var inventory = await _unitOfWork.Inventory.Get(u => u.Id == inventoryId);
        inventory.LastRestocked = new DateOnly(dt.Year, dt.Month, dt.Day);
        _unitOfWork.Inventory.Update(inventory);

        var stock = new Stock()
        {
            InventoryId = inventoryId,
            ExpiryDate= new DateOnly(restock.ExpiryDate.Year, restock.ExpiryDate.Month, restock.ExpiryDate.Day),
            Quantity= restock.Quantity,
            CreatedAt = new DateOnly(dt.Year, dt.Month, dt.Day),
        };

        await _unitOfWork.Stock.Add(stock);
        await _unitOfWork.SaveAsync();
    }
}
