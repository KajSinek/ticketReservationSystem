namespace Abstractions.BackgroundJobs;

public interface IBackgroundTaskQueue
{
    void Queue(Func<CancellationToken, Task> task);
    Task<Func<CancellationToken, Task>?> DequeueAsync(CancellationToken ct = default);
}
