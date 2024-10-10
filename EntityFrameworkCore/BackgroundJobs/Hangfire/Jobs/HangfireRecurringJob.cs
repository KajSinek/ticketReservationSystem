using Abstractions.BackgroundJobs.Hangfire.Models;
using Microsoft.Extensions.Logging;

namespace VismaIdella.Vips.Core.BackgroundJobs.Hangfire.Jobs;

public abstract class HangfireRecurringJob<TData>(ILogger logger) : HangfireJob<TData>(logger)
    where TData : HangfireRecurringJobData
{
    public override Task ExecuteAsync(TData request, CancellationToken ct = default)
    {
        Logger.LogInformation(
            "Executing recurring job {JobName} for tenant with id {TenantId} and identifier {JobIdentifier} that is scheduled with cron {JobCron}",
            JobName,
            request.TenantId,
            request.RecurringJobIdentifier,
            request.Cron
        );
        return OnExecuteAsync(request, ct);
    }
}
