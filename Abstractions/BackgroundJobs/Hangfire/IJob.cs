using System.Threading;
using System.Threading.Tasks;
using Abstractions.BackgroundJobs.Hangfire.Models;

namespace Abstractions.BackgroundJobs.Hangfire;

public interface IJob
{
    string JobName { get; }
}

public interface IJob<in TData> : IJob
    where TData : HangfireJobData
{
    Task ExecuteAsync(TData request, CancellationToken ct = default);
}
