namespace FactoryManagementSystem1.Domain.Entities;

public class InventoryItem
{
    public int Id { get; set; }

    public string SKU { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty; // e.g. kg, pcs

    public decimal OnHandQty { get; set; }

    public decimal Threshold { get; set; } // low stock alert
}
