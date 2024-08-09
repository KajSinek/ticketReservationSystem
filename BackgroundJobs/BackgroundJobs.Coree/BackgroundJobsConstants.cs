namespace BackgroundJobs.Core;

public static class BackgroundJobsConstants
{
    public static class ConnectionStrings
    {
        public const string HangfireConnectionString = "HangfireConnection";

        public static string GetConnectionStringFullPath(string connectionString)
        {
            return $"ConnectionStrings:{connectionString}";
        }
    }
}
