namespace CronTools;

/// <summary>
/// Extension methods for CronBuilder
/// </summary>
public static class CronExtensions
{
    /// <summary>
    /// Creates a Cron expression for daily execution at a specific time
    /// </summary>
    /// <param name="builder">The CronBuilder instance</param>
    /// <param name="hour">Hour (0-23)</param>
    /// <param name="minute">Minute (0-59)</param>
    /// <param name="second">Second (0-59, optional, defaults to 0)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public static CronBuilder DailyAt(this CronBuilder builder, int hour, int minute, int second = 0)
    {
        return builder.AtSecond(second).AtMinute(minute).AtHour(hour).Daily();
    }

    /// <summary>
    /// Creates a Cron expression for weekly execution on a specific day and time
    /// </summary>
    /// <param name="builder">The CronBuilder instance</param>
    /// <param name="dayOfWeek">Day of week (1-7, where 1=Monday)</param>
    /// <param name="hour">Hour (0-23)</param>
    /// <param name="minute">Minute (0-59)</param>
    /// <param name="second">Second (0-59, optional, defaults to 0)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public static CronBuilder WeeklyAt(this CronBuilder builder, int dayOfWeek, int hour, int minute, int second = 0)
    {
        return builder.AtSecond(second).AtMinute(minute).AtHour(hour).OnWeekday(dayOfWeek);
    }

    /// <summary>
    /// Creates a Cron expression for monthly execution on a specific day and time
    /// </summary>
    /// <param name="builder">The CronBuilder instance</param>
    /// <param name="dayOfMonth">Day of month (1-31)</param>
    /// <param name="hour">Hour (0-23)</param>
    /// <param name="minute">Minute (0-59)</param>
    /// <param name="second">Second (0-59, optional, defaults to 0)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public static CronBuilder MonthlyAt(this CronBuilder builder, int dayOfMonth, int hour, int minute, int second = 0)
    {
        return builder.AtSecond(second).AtMinute(minute).AtHour(hour).OnDay(dayOfMonth);
    }

    /// <summary>
    /// Creates a Cron expression for execution at the start of every hour
    /// </summary>
    /// <param name="builder">The CronBuilder instance</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public static CronBuilder AtStartOfHour(this CronBuilder builder)
    {
        return builder.AtSecond(0).AtMinute(0).Hourly();
    }

    /// <summary>
    /// Creates a Cron expression for execution at the start of every day (midnight)
    /// </summary>
    /// <param name="builder">The CronBuilder instance</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public static CronBuilder AtMidnight(this CronBuilder builder)
    {
        return builder.DailyAt(0, 0, 0);
    }

    /// <summary>
    /// Creates a Cron expression for execution at noon
    /// </summary>
    /// <param name="builder">The CronBuilder instance</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public static CronBuilder AtNoon(this CronBuilder builder)
    {
        return builder.DailyAt(12, 0, 0);
    }

    /// <summary>
    /// Creates a Cron expression for execution on the first day of every month
    /// </summary>
    /// <param name="builder">The CronBuilder instance</param>
    /// <param name="hour">Hour (0-23, optional, defaults to 0)</param>
    /// <param name="minute">Minute (0-59, optional, defaults to 0)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public static CronBuilder OnFirstDayOfMonth(this CronBuilder builder, int hour = 0, int minute = 0)
    {
        return builder.MonthlyAt(1, hour, minute);
    }

    /// <summary>
    /// Creates a Cron expression for execution on the last day of every month
    /// </summary>
    /// <param name="builder">The CronBuilder instance</param>
    /// <param name="hour">Hour (0-23, optional, defaults to 0)</param>
    /// <param name="minute">Minute (0-59, optional, defaults to 0)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public static CronBuilder OnLastDayOfMonth(this CronBuilder builder, int hour = 0, int minute = 0)
    {
        // Note: This uses day 31, which may not exist in all months
        // A more sophisticated implementation would use L syntax if supported
        return builder.AtSecond(0).AtMinute(minute).AtHour(hour).OnDay(31);
    }

    /// <summary>
    /// Validates the built Cron expression
    /// </summary>
    /// <param name="builder">The CronBuilder instance</param>
    /// <returns>True if the expression is valid, false otherwise</returns>
    public static bool IsValid(this CronBuilder builder)
    {
        try
        {
            var expression = builder.Build();
            return CronParser.IsValid(expression);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the next execution time for the built Cron expression
    /// </summary>
    /// <param name="builder">The CronBuilder instance</param>
    /// <param name="fromTime">The time to calculate from (optional, defaults to now)</param>
    /// <returns>The next execution time</returns>
    public static DateTime GetNextExecutionTime(this CronBuilder builder, DateTime? fromTime = null)
    {
        var expression = builder.Build();
        return CronParser.GetNextExecutionTime(expression, fromTime);
    }
}