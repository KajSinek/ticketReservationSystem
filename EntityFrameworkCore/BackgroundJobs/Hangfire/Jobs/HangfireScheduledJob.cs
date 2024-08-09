using Abstractions.BackgroundJobs.Hangfire.Models;
using Microsoft.Extensions.Logging;

namespace VismaIdella.Vips.Core.BackgroundJobs.Hangfire.Jobs;

public abstract class HangfireScheduledJob<TData>(ILogger logger) : HangfireJob<TData>(logger)
    where TData : HangfireJobData
{
    public override Task ExecuteAsync(TData request, CancellationToken ct = default)
    {
        Logger.LogInformation(
            "Executing scheduled job {JobName} for tenant with id {TenantId}",
            JobName,
            request.TenantId
        );
        return OnExecuteAsync(request, ct);
    }
}
