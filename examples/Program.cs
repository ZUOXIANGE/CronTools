namespace CronTools.Examples;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("CronBuilder Examples");
        Console.WriteLine("===================");
        Console.WriteLine();

        Console.WriteLine("Basic Examples:");
        Console.WriteLine("--------------");

        // Daily at 8:30 AM
        var daily = CronTools.CronBuilder.Create()
            .AtMinute(30)
            .AtHour(8)
            .Build();
        Console.WriteLine($"Daily at 8:30 AM: {daily}");

        // Every Monday at 9:00 AM
        var weekly = CronTools.CronBuilder.Create()
            .AtMinute(0)
            .AtHour(9)
            .OnMonday()
            .Build();
        Console.WriteLine($"Every Monday at 9:00 AM: {weekly}");

        // 15th of every month at 2:30 PM
        var monthly = CronTools.CronBuilder.Create()
            .AtMinute(30)
            .AtHour(14)
            .OnDay(15)
            .Build();
        Console.WriteLine($"15th of every month at 2:30 PM: {monthly}");

        // Every 5 minutes
        var everyFiveMinutes = CronTools.CronBuilder.Create()
            .EveryNMinutes(5)
            .Build();
        Console.WriteLine($"Every 5 minutes: {everyFiveMinutes}");

        Console.WriteLine();
        Console.WriteLine("Advanced Examples:");
        Console.WriteLine("-----------------");

        // Weekdays at 9 AM and 5 PM
        var weekdaysMultipleTimes = CronTools.CronBuilder.Create()
            .AtMinute(0)
            .AtHours(9, 17)
            .OnWeekdays(1, 2, 3, 4, 5)
            .Build();
        Console.WriteLine($"Weekdays at 9 AM and 5 PM: {weekdaysMultipleTimes}");

        // Complex schedule
        var complex = CronTools.CronBuilder.Create()
            .AtSecond(30)
            .AtMinutes(0, 15, 30, 45)
            .AtHours(9, 12, 15, 18)
            .OnDays(5, 15, 25)
            .InMonths(3, 6, 9, 12)
            .Build();
        Console.WriteLine($"Complex schedule: {complex}");

        // Days 10-20 of months 4-10 at 2 PM
        var rangeExample = CronTools.CronBuilder.Create()
            .AtMinute(0)
            .AtHour(14)
            .OnDaysBetween(10, 20)
            .InMonthsBetween(4, 10)
            .Build();
        Console.WriteLine($"Days 10-20 of months 4-10 at 2 PM: {rangeExample}");

        // Every 2 hours on weekdays
        var everyTwoHours = CronTools.CronBuilder.Create()
            .AtMinute(0)
            .EveryNHours(2)
            .OnWeekdays(1, 2, 3, 4, 5)
            .Build();
        Console.WriteLine($"Every 2 hours on weekdays: {everyTwoHours}");

        Console.WriteLine();
        Console.WriteLine("Extension Method Examples:");
        Console.WriteLine("--------------------------");

        // Daily at noon
        var dailyNoon = CronTools.CronBuilder.Create().DailyAt(12, 0).Build();
        Console.WriteLine($"Daily at noon: {dailyNoon}");

        // Daily at midnight
        var dailyMidnight = CronTools.CronBuilder.Create().DailyAt(0, 0).Build();
        Console.WriteLine($"Daily at midnight: {dailyMidnight}");

        // Weekly meeting (Monday 10 AM)
        var weeklyMeeting = CronTools.CronBuilder.Create().WeeklyAt(1, 10, 0).Build();
        Console.WriteLine($"Weekly meeting (Monday 10 AM): {weeklyMeeting}");

        // Monthly report (1st at 9 AM)
        var monthlyReport = CronTools.CronBuilder.Create().MonthlyAt(1, 9, 0).Build();
        Console.WriteLine($"Monthly report (1st at 9 AM): {monthlyReport}");

        // First day of month at 8:30 AM
        var firstDayOfMonth = CronTools.CronBuilder.Create()
            .AtMinute(30)
            .AtHour(8)
            .OnDay(1)
            .Build();
        Console.WriteLine($"First day of month at 8:30 AM: {firstDayOfMonth}");

        Console.WriteLine();
        Console.WriteLine("Preset Examples:");
        Console.WriteLine("---------------");

        // Common presets
        var everyMinute = CronTools.CronBuilder.Create().EveryMinute().Build();
        Console.WriteLine($"Every minute: {everyMinute}");

        var everyFiveMin = CronTools.CronBuilder.Create().EveryNMinutes(5).Build();
        Console.WriteLine($"Every 5 minutes: {everyFiveMin}");

        var everyFifteenMin = CronTools.CronBuilder.Create().EveryNMinutes(15).Build();
        Console.WriteLine($"Every 15 minutes: {everyFifteenMin}");

        var hourly = CronTools.CronBuilder.Create().Hourly().Build();
        Console.WriteLine($"Every hour: {hourly}");

        var everySixHours = CronTools.CronBuilder.Create().EveryNHours(6).Build();
        Console.WriteLine($"Every 6 hours: {everySixHours}");

        var dailyDefault = CronTools.CronBuilder.Create().Daily().Build();
        Console.WriteLine($"Daily: {dailyDefault}");

        var weeklyDefault = CronTools.CronBuilder.Create().AtHour(9).OnMonday().Build();
        Console.WriteLine($"Weekly (Monday): {weeklyDefault}");

        var monthlyDefault = CronTools.CronBuilder.Create().OnDay(1).Build();
        Console.WriteLine($"Monthly: {monthlyDefault}");

        var weekdaysAt9 = CronTools.CronBuilder.Create().AtHour(9).OnWeekdays().Build();
        Console.WriteLine($"Weekdays at 9 AM: {weekdaysAt9}");

        var weekendsAt10 = CronTools.CronBuilder.Create().AtHour(10).OnWeekends().Build();
        Console.WriteLine($"Weekends at 10 AM: {weekendsAt10}");

        Console.WriteLine();
        Console.WriteLine("Validation Examples:");
        Console.WriteLine("-------------------");

        // Validate a cron expression
        var testExpression = "0 30 9 * * ? *";
        var isValid = CronParser.IsValid(testExpression);
        Console.WriteLine($"Valid expression: {testExpression}");
        Console.WriteLine($"Is valid: {isValid}");
        Console.WriteLine();

        // Parse and validate an existing expression
        var existingExpression = "0 30 9 * * ? *";
        var validationResult = CronParser.IsValid(existingExpression);
        Console.WriteLine($"Existing expression '{existingExpression}' is valid: {validationResult}");

        if (validationResult)
        {
            var parsed = CronParser.Parse(existingExpression);
            Console.WriteLine("Parsed components:");
            Console.WriteLine($"  Seconds: {parsed.Seconds}");
            Console.WriteLine($"  Minutes: {parsed.Minutes}");
            Console.WriteLine($"  Hours: {parsed.Hours}");
            Console.WriteLine($"  Day of Month: {parsed.DayOfMonth}");
            Console.WriteLine($"  Month: {parsed.Month}");
            Console.WriteLine($"  Day of Week: {parsed.DayOfWeek}");
            Console.WriteLine($"  Year: {parsed.Year}");
        }

        Console.WriteLine();
        Console.WriteLine("CronBuilder Chinese Description Examples:");
        Console.WriteLine("----------------------------------------");

        // 演示CronBuilder的中文描述功能
        var builders = new[]
        {
            ("每天", CronTools.CronBuilder.Create()),
            ("每天上午8点", CronTools.CronBuilder.Create().AtHour(8)),
            ("每周一", CronTools.CronBuilder.Create().OnMonday()),
            ("每周一和周三下午3点", CronTools.CronBuilder.Create().AtHour(15).OnWeekdays(1, 3)),
            ("每月15号", CronTools.CronBuilder.Create().OnDay(15)),
            ("每5分钟", CronTools.CronBuilder.Create().EveryNMinutes(5)),
            ("每2小时在30分", CronTools.CronBuilder.Create().AtMinute(30).EveryNHours(2)),
            ("每3天", CronTools.CronBuilder.Create().EveryNDays(3)),
            ("每周一、三、五", CronTools.CronBuilder.Create().OnWeekdays(1, 3, 5))
        };

        foreach (var (expectedDesc, builder) in builders)
        {
            var cronExpr = builder.Build();
            var actualDesc = builder.GetChineseDescription();
            if (expectedDesc != actualDesc)
            {
                Console.WriteLine($"Error: {cronExpr} => {actualDesc}");
                continue;
            }
            Console.WriteLine($"{cronExpr} => {actualDesc}");
        }

        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}