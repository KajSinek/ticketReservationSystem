namespace Abstractions.BackgroundJobs.Hangfire.Models;

public class HangfireRecurringJobData : HangfireJobData
{
    public required string Cron { get; set; }

    public required string RecurringJobIdentifier { get; set; }
}
