namespace Abstractions.BackgroundJobs.Hangfire.Models;

public class HangfireTenantApiModel
{
    public required string TenantId { get; set; } = null!;
}
