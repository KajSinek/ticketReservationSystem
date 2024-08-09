namespace Abstractions.BackgroundJobs.Hangfire.Models;

public abstract class HangfireJobData
{
    public required string TenantId { get; set; }
}
