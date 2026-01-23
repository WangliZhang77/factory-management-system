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

    /// <summary>
    /// Create a Draft work order with an auto-generated WorkOrderNo.
    /// Status: Draft
    /// </summary>
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

        // Use transaction to keep WorkOrder + generated number consistent
        await using var tx = await _db.Database.BeginTransactionAsync();

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

        await tx.CommitAsync();
        return workOrder;
    }

    /// <summary>
    /// Submit a work order.
    /// Rule: only Draft can be submitted.
    /// Writes an AuditLog entry for traceability.
    /// </summary>
    public async Task SubmitAsync(int workOrderId, string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new InvalidOperationException("UserId is required.");

        // Transaction ensures status change + audit log are saved together
        await using var tx = await _db.Database.BeginTransactionAsync();

        var wo = await _db.WorkOrders
            .FirstOrDefaultAsync(w => w.Id == workOrderId);

        if (wo == null)
            throw new InvalidOperationException("Work order not found.");

        // ⭐ State machine rule (interview point)
        if (wo.Status != WorkOrderStatus.Draft)
            throw new InvalidOperationException("Only Draft work orders can be submitted.");

        wo.Status = WorkOrderStatus.Submitted;
        wo.SubmittedAtUtc = DateTime.UtcNow;
        wo.SubmittedByUserId = userId;

        _db.AuditLogs.Add(new AuditLog
        {
            UserId = userId,
            Action = "WorkOrderSubmitted",
            EntityName = "WorkOrder",
            EntityId = wo.Id.ToString(),
            Details = $"WorkOrderNo={wo.WorkOrderNo}",
            TimestampUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
    }

    /// <summary>
    /// MVP WorkOrderNo generator: WO-{year}-{seq:D4}
    /// NOTE: This is fine for a demo. For concurrency-safe production,
    /// use a database sequence or unique constraint + retry.
    /// </summary>
    private async Task<string> GenerateWorkOrderNoAsync()
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"WO-{year}-";

        // Find the max existing sequence number for this year
        // Example existing: WO-2026-0007 => seq=7
        var existingNos = await _db.WorkOrders
            .Where(w => w.WorkOrderNo.StartsWith(prefix))
            .Select(w => w.WorkOrderNo)
            .ToListAsync();

        var maxSeq = 0;
        foreach (var no in existingNos)
        {
            // Safe parse: WO-YYYY-#### (length check optional)
            var parts = no.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out var seq))
            {
                if (seq > maxSeq) maxSeq = seq;
            }
        }

        var next = maxSeq + 1;
        return $"{prefix}{next:D4}";
    }
}
