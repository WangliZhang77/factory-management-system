using FactoryManagementSystem1.Data;
using FactoryManagementSystem1.Domain.Entities;

namespace FactoryManagementSystem1.Services;

public class AuditService
{
    private readonly ApplicationDbContext _db;

    public AuditService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task LogAsync(
        string userId,
        string action,
        string entityName,
        string entityId,
        string details)
    {
        var log = new AuditLog
        {
            UserId = userId,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Details = details,
            TimestampUtc = DateTime.UtcNow
        };

        _db.AuditLogs.Add(log);
        await _db.SaveChangesAsync();
    }
}
