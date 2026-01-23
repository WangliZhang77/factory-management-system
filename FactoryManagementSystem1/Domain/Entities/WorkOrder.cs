using FactoryManagementSystem1.Domain.Enums;

namespace FactoryManagementSystem1.Domain.Entities;

public class WorkOrder
{
    public DateTime? SubmittedAtUtc { get; set; }
    public string? SubmittedByUserId { get; set; }

    public int Id { get; set; }

    // Human-friendly number, e.g. WO-2026-0001 (we can generate later)
    public string WorkOrderNo { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public DateTime? PlannedStart { get; set; }
    public DateTime? PlannedEnd { get; set; }

    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Draft;

    // Optional assignment (link to AspNetUsers.Id later)
    public string? AssignedToUserId { get; set; }

    // Optional equipment
    public int? EquipmentId { get; set; }
}
