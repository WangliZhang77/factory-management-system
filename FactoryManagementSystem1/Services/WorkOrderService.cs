using FactoryManagementSystem1.Data;
using FactoryManagementSystem1.Domain.Entities;
using FactoryManagementSystem1.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FactoryManagementSystem1.Services;

public class WorkOrderService
{
    private readonly ApplicationDbContext _db;

    public WorkOrderService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<WorkOrder> CreateDraftAsync(
        string title,
        int quantity,
        DateTime? plannedStart,
        DateTime? plannedEnd)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidOperationException("Title is required.");

        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero.");

        var woNo = await GenerateWorkOrderNoAsync();

        var workOrder = new WorkOrder
        {
            WorkOrderNo = woNo,
            Title = title.Trim(),
            Quantity = quantity,
            PlannedStart = plannedStart,
            PlannedEnd = plannedEnd,
            Status = WorkOrderStatus.Draft
        };

        _db.WorkOrders.Add(workOrder);
        await _db.SaveChangesAsync();

        return workOrder;
    }

    private async Task<string> GenerateWorkOrderNoAsync()
    {
        // Simple sequential number based on existing count (OK for demo MVP).
        // Later we can improve with a database sequence for concurrency.
        var year = DateTime.UtcNow.Year;

        var countThisYear = await _db.WorkOrders
            .CountAsync(w => w.WorkOrderNo.StartsWith($"WO-{year}-"));

        var next = countThisYear + 1;

        return $"WO-{year}-{next:D4}";
    }
}
