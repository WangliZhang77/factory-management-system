using FactoryManagementSystem1.Data;
using FactoryManagementSystem1.Domain.Entities;
using FactoryManagementSystem1.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FactoryManagementSystem1.Services;

public class InventoryService
{
    private readonly ApplicationDbContext _db;

    public InventoryService(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task IssueStockAsync(
        int inventoryItemId,
        decimal quantity,
        string reason,
        string userId,
        int? workOrderId = null)
    {
        var item = await _db.InventoryItems
            .FirstOrDefaultAsync(i => i.Id == inventoryItemId);

        if (item == null)
        {
            throw new InvalidOperationException("Inventory item not found.");
        }

        if (quantity <= 0)
        {
            throw new InvalidOperationException("Quantity must be greater than zero.");
        }

        if (item.OnHandQty - quantity < 0)
        {
            throw new InvalidOperationException("Insufficient stock. Inventory cannot go negative.");
        }

    
        item.OnHandQty -= quantity;
        var movement = new StockMovement
        {
            InventoryItemId = item.Id,
            Type = StockMovementType.Out,
            Qty = quantity,
            Reason = reason,
            CreatedByUserId = userId,
            WorkOrderId = workOrderId
        };

        _db.StockMovements.Add(movement);

        await _db.SaveChangesAsync();
    }
}
