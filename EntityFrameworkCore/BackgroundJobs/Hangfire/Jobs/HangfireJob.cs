using System.Threading;
using System.Threading.Tasks;
using Abstractions.BackgroundJobs.Hangfire;
using Abstractions.BackgroundJobs.Hangfire.Models;
using Microsoft.Extensions.Logging;

namespace VismaIdella.Vips.Core.BackgroundJobs.Hangfire.Jobs;

public abstract class HangfireJob<TData>(ILogger logger) : IJob<TData>
    where TData : HangfireJobData
{
    public string JobName => GetType().Name;

    protected ILogger Logger { get; } = logger;

    public virtual Task ExecuteAsync(TData request, CancellationToken ct = default)
    {
        Logger.LogInformation(
            "Executing job {JobName} for tenant {TenantId}",
            JobName,
            request.TenantId
        );
        return OnExecuteAsync(request, ct);
    }

    public abstract Task OnExecuteAsync(TData request, CancellationToken ct = default);
}
