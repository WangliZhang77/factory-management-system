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

    // ============================================================
    // Create Draft
    // ============================================================

    /// <summary>
    /// Create a Draft work order with an auto-generated WorkOrderNo.
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

    // ============================================================
    // Submit
    // ============================================================

    /// <summary>
    /// Submit a Draft work order.
    /// Rule: only Draft can be submitted.
    /// </summary>
    public async Task SubmitAsync(int workOrderId, string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new InvalidOperationException("UserId is required.");

        await using var tx = await _db.Database.BeginTransactionAsync();

        var wo = await _db.WorkOrders.FirstOrDefaultAsync(w => w.Id == workOrderId);
        if (wo == null)
            throw new InvalidOperationException("Work order not found.");

        if (wo.Status == WorkOrderStatus.Submitted)
            throw new InvalidOperationException("Work order has already been submitted.");

        if (wo.Status != WorkOrderStatus.Draft)
            throw new InvalidOperationException(
                $"Work order cannot be submitted from status '{wo.Status}'.");

        wo.Status = WorkOrderStatus.Submitted;
        wo.SubmittedAtUtc = DateTime.UtcNow;
        wo.SubmittedByUserId = userId;

        var hasSubmitLog = await _db.AuditLogs.AnyAsync(a =>
            a.EntityName == "WorkOrder" &&
            a.EntityId == wo.Id.ToString() &&
            a.Action == "WorkOrderSubmitted");

        if (!hasSubmitLog)
        {
            _db.AuditLogs.Add(new AuditLog
            {
                UserId = userId,
                Action = "WorkOrderSubmitted",
                EntityName = "WorkOrder",
                EntityId = wo.Id.ToString(),
                Details = $"WorkOrderNo={wo.WorkOrderNo}",
                TimestampUtc = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
    }

    // ============================================================
    // Approve / Reject
    // ============================================================

    public async Task ApproveAsync(int workOrderId, string supervisorUserId)
    {
        if (string.IsNullOrWhiteSpace(supervisorUserId))
            throw new InvalidOperationException("Supervisor userId is required.");

        await using var tx = await _db.Database.BeginTransactionAsync();

        var wo = await _db.WorkOrders.FirstOrDefaultAsync(w => w.Id == workOrderId);
        if (wo == null)
            throw new InvalidOperationException("Work order not found.");

        if (wo.Status != WorkOrderStatus.Submitted)
            throw new InvalidOperationException(
                $"Only Submitted work orders can be approved. Current status: {wo.Status}");

        wo.Status = WorkOrderStatus.Approved;
        wo.ApprovedAtUtc = DateTime.UtcNow;
        wo.ApprovedByUserId = supervisorUserId;

        _db.AuditLogs.Add(new AuditLog
        {
            UserId = supervisorUserId,
            Action = "WorkOrderApproved",
            EntityName = "WorkOrder",
            EntityId = wo.Id.ToString(),
            Details = $"WorkOrderNo={wo.WorkOrderNo}",
            TimestampUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
    }

    public async Task RejectAsync(int workOrderId, string supervisorUserId, string reason)
    {
        if (string.IsNullOrWhiteSpace(supervisorUserId))
            throw new InvalidOperationException("Supervisor userId is required.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new InvalidOperationException("Rejection reason is required.");

        await using var tx = await _db.Database.BeginTransactionAsync();

        var wo = await _db.WorkOrders.FirstOrDefaultAsync(w => w.Id == workOrderId);
        if (wo == null)
            throw new InvalidOperationException("Work order not found.");

        if (wo.Status != WorkOrderStatus.Submitted)
            throw new InvalidOperationException(
                $"Only Submitted work orders can be rejected. Current status: {wo.Status}");

        wo.Status = WorkOrderStatus.Rejected;
        wo.RejectedAtUtc = DateTime.UtcNow;
        wo.RejectedByUserId = supervisorUserId;
        wo.RejectionReason = reason.Trim();

        _db.AuditLogs.Add(new AuditLog
        {
            UserId = supervisorUserId,
            Action = "WorkOrderRejected",
            EntityName = "WorkOrder",
            EntityId = wo.Id.ToString(),
            Details = $"Reason={reason}",
            TimestampUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
    }

    // ============================================================
    // In Progress → Completed
    // ============================================================

    public async Task CompleteAsync(int workOrderId, string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new InvalidOperationException("UserId is required.");

        await using var tx = await _db.Database.BeginTransactionAsync();

        var wo = await _db.WorkOrders.FirstOrDefaultAsync(w => w.Id == workOrderId);
        if (wo == null)
            throw new InvalidOperationException("Work order not found.");

        if (wo.Status != WorkOrderStatus.InProgress)
            throw new InvalidOperationException(
                $"Only InProgress work orders can be completed. Current status: {wo.Status}");

        wo.Status = WorkOrderStatus.Completed;
        wo.CompletedAtUtc = DateTime.UtcNow;
        wo.CompletedByUserId = userId;

        _db.AuditLogs.Add(new AuditLog
        {
            UserId = userId,
            Action = "WorkOrderCompleted",
            EntityName = "WorkOrder",
            EntityId = wo.Id.ToString(),
            Details = $"WorkOrderNo={wo.WorkOrderNo}",
            TimestampUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
    }

    // ============================================================
    // Helpers
    // ============================================================

    /// <summary>
    /// MVP WorkOrderNo generator: WO-{year}-{seq:D4}
    /// </summary>
    private async Task<string> GenerateWorkOrderNoAsync()
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"WO-{year}-";

        var existingNos = await _db.WorkOrders
            .Where(w => w.WorkOrderNo.StartsWith(prefix))
            .Select(w => w.WorkOrderNo)
            .ToListAsync();

        var maxSeq = 0;
        foreach (var no in existingNos)
        {
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
