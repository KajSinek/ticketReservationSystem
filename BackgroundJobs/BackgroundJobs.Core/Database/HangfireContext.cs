using Microsoft.EntityFrameworkCore;

namespace BackgroundJobs.Core.Database;

public class HangfireContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HangfireContext"/> class.
    /// </summary>
    /// <param name="options">DB context options.</param>
    public HangfireContext(DbContextOptions<HangfireContext> options)
        : base(options) { }
}
