using FactoryManagementSystem1.Domain.Enums;

namespace FactoryManagementSystem1.Domain.Entities;

public class WorkOrder
{
    public int Id { get; set; }

    // ===== Core Info =====
    // Human-friendly number, e.g. WO-2026-0001
    public string WorkOrderNo { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public DateTime? PlannedStart { get; set; }
    public DateTime? PlannedEnd { get; set; }

    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Draft;

    // ===== Submission =====
    public DateTime? SubmittedAtUtc { get; set; }
    public string? SubmittedByUserId { get; set; }

    // ===== Approval =====
    public DateTime? ApprovedAtUtc { get; set; }
    public string? ApprovedByUserId { get; set; }

    // ===== Rejection =====
    public DateTime? RejectedAtUtc { get; set; }
    public string? RejectedByUserId { get; set; }
    public string? RejectionReason { get; set; }

    // ===== Completion =====
    public DateTime? CompletedAtUtc { get; set; }
    public string? CompletedByUserId { get; set; }

    // ===== Cancellation =====
    public DateTime? CancelledAtUtc { get; set; }
    public string? CancelledByUserId { get; set; }

    // ===== Optional Assignment =====
    // Link to AspNetUsers.Id
    public string? AssignedToUserId { get; set; }

    // ===== Optional Equipment =====
    public int? EquipmentId { get; set; }
}
