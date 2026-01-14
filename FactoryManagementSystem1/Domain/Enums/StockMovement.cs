using FactoryManagementSystem1.Domain.Enums;

namespace FactoryManagementSystem1.Domain.Entities;

public class StockMovement
{
    public int Id { get; set; }

    public StockMovementType Type { get; set; }

    public decimal Qty { get; set; }

    public string Reason { get; set; } = string.Empty;

    // Foreign keys
    public int InventoryItemId { get; set; }
    public InventoryItem InventoryItem { get; set; } = null!;

    // Optional link to a work order later
    public int? WorkOrderId { get; set; }

    // Audit fields
    public string CreatedByUserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
