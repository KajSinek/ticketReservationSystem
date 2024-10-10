namespace Abstractions.BackgroundJobs.Hangfire;

public interface IHangfireJobProvider
{
    TJob GetJob<TJob>(string? tenantId = null)
        where TJob : IJob;
}
