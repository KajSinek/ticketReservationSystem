using Abstractions.BackgroundJobs.Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.BackgroundJobs.Hangfire;

public class HangfireJobProvider(
    IServiceProvider serviceProvider,
    ILogger<HangfireJobProvider> logger
) : IHangfireJobProvider
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<HangfireJobProvider> _logger = logger;

    public TJob GetJob<TJob>(string? tenantId = null)
        where TJob : IJob
    {
        _logger.LogDebug("Getting job {JobType}", typeof(TJob).Name);
        var job = _serviceProvider.GetRequiredService<TJob>();
        return job;
    }
}
