namespace FactoryManagementSystem1.Domain.Entities;

public class AuditLog
{
    public long Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty; // e.g., "StockOut"

    public string EntityName { get; set; } = string.Empty; // e.g., "InventoryItem"

    public string EntityId { get; set; } = string.Empty; // store as string for flexibility

    public string Details { get; set; } = string.Empty; // free-text / JSON later

    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}
