namespace EntityFrameworkCore.Configuration;

public class EntityFrameworkCoreOptions
{
    public string? MigrationAssembly { get; set; }
    public int MaxRetryCount { get; set; }
    public int RetryDelayInSeconds { get; set; }
}
