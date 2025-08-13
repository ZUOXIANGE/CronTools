namespace CronTools;

/// <summary>
/// Predefined Cron expressions for common scheduling scenarios
/// </summary>
public static class CronPresets
{
    /// <summary>
    /// Every minute: "0 * * * * ? *"
    /// </summary>
    public static string EveryMinute => CronBuilder.Create().EveryMinute().Build();

    /// <summary>
    /// Every 5 minutes: "0 */5 * * * ? *"
    /// </summary>
    public static string Every5Minutes => CronBuilder.Create().EveryNMinutes(5).Build();

    /// <summary>
    /// Every 15 minutes: "0 */15 * * * ? *"
    /// </summary>
    public static string Every15Minutes => CronBuilder.Create().EveryNMinutes(15).Build();

    /// <summary>
    /// Every 30 minutes: "0 */30 * * * ? *"
    /// </summary>
    public static string Every30Minutes => CronBuilder.Create().EveryNMinutes(30).Build();

    /// <summary>
    /// Every hour: "0 0 * * * ? *"
    /// </summary>
    public static string EveryHour => CronBuilder.Create().Hourly().Build();

    /// <summary>
    /// Every 2 hours: "0 0 */2 * * ? *"
    /// </summary>
    public static string Every2Hours => CronBuilder.Create().EveryNHours(2).Build();

    /// <summary>
    /// Every 6 hours: "0 0 */6 * * ? *"
    /// </summary>
    public static string Every6Hours => CronBuilder.Create().EveryNHours(6).Build();

    /// <summary>
    /// Every 12 hours: "0 0 */12 * * ? *"
    /// </summary>
    public static string Every12Hours => CronBuilder.Create().EveryNHours(12).Build();

    /// <summary>
    /// Daily at midnight: "0 0 0 * * ? *"
    /// </summary>
    public static string Daily => CronBuilder.Create().AtMidnight().Build();

    /// <summary>
    /// Weekly on Monday at 9 AM: "0 0 9 ? * 1 *"
    /// </summary>
    public static string WeeklyMonday => CronBuilder.Create().WeeklyAt(1, 9, 0).Build();

    /// <summary>
    /// Monthly on the 1st at midnight: "0 0 0 1 * ? *"
    /// </summary>
    public static string Monthly => CronBuilder.Create().OnFirstDayOfMonth().Build();

    /// <summary>
    /// Weekdays at 9 AM: "0 0 9 ? * 1-5 *"
    /// </summary>
    public static string WeekdaysAt9AM => CronBuilder.Create().AtSecond(0).AtMinute(0).AtHour(9).OnWeekdays().Build();

    /// <summary>
    /// Weekends at 10 AM: "0 0 10 ? * 6,7 *"
    /// </summary>
    public static string WeekendsAt10AM => CronBuilder.Create().AtSecond(0).AtMinute(0).AtHour(10).OnWeekends().Build();
}