namespace Helpers.Utilities;

public class DateHelper
{
    public static DateTime DateTimeUtcNow { get; set; } = DateTime.UtcNow;
    public static DateOnly DateOnlyUtcNow { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public static TimeOnly TimeOnlyUtcNow { get; set; } = TimeOnly.FromDateTime(DateTime.UtcNow);

}
