namespace CronTools;

/// <summary>
/// Represents a parsed Cron expression
/// </summary>
public class CronExpression
{
    /// <summary>
    /// Gets or sets the seconds field (0-59)
    /// </summary>
    public string Seconds { get; set; } = "0";

    /// <summary>
    /// Gets or sets the minutes field (0-59)
    /// </summary>
    public string Minutes { get; set; } = "0";

    /// <summary>
    /// Gets or sets the hours field (0-23)
    /// </summary>
    public string Hours { get; set; } = "0";

    /// <summary>
    /// Gets or sets the day of month field (1-31)
    /// </summary>
    public string DayOfMonth { get; set; } = "*";

    /// <summary>
    /// Gets or sets the month field (1-12)
    /// </summary>
    public string Month { get; set; } = "*";

    /// <summary>
    /// Gets or sets the day of week field (1-7)
    /// </summary>
    public string DayOfWeek { get; set; } = "?";

    /// <summary>
    /// Gets or sets the year field (1970-2099)
    /// </summary>
    public string Year { get; set; } = "*";

    /// <summary>
    /// Returns the Cron expression as a string
    /// </summary>
    /// <returns>The Cron expression string</returns>
    public override string ToString()
    {
        return $"{Seconds} {Minutes} {Hours} {DayOfMonth} {Month} {DayOfWeek} {Year}";
    }
}