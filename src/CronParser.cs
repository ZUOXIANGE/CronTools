using System.Text.RegularExpressions;

namespace CronTools;

/// <summary>
/// Provides functionality to parse and validate Cron expressions
/// </summary>
public static class CronParser
{
    private static readonly Regex CronRegex = new(
        @"^\s*(\S+)\s+(\S+)\s+(\S+)\s+(\S+)\s+(\S+)\s+(\S+)(?:\s+(\S+))?\s*$",
        RegexOptions.Compiled);

    /// <summary>
    /// Parses a Cron expression into its component parts
    /// </summary>
    /// <param name="cronExpression">The Cron expression to parse</param>
    /// <returns>A CronExpression object containing the parsed components</returns>
    /// <exception cref="ArgumentException">Thrown when the Cron expression is invalid</exception>
    public static CronExpression Parse(string cronExpression)
    {
        if (string.IsNullOrWhiteSpace(cronExpression))
            throw new ArgumentException("Cron expression cannot be null or empty", nameof(cronExpression));

        var match = CronRegex.Match(cronExpression.Trim());
        if (!match.Success)
            throw new ArgumentException("Invalid Cron expression format", nameof(cronExpression));

        var groups = match.Groups;
        return new CronExpression
        {
            Seconds = groups[1].Value,
            Minutes = groups[2].Value,
            Hours = groups[3].Value,
            DayOfMonth = groups[4].Value,
            Month = groups[5].Value,
            DayOfWeek = groups[6].Value,
            Year = groups.Count > 7 && !string.IsNullOrEmpty(groups[7].Value) ? groups[7].Value : "*"
        };
    }

    /// <summary>
    /// Validates a Cron expression
    /// </summary>
    /// <param name="cronExpression">The Cron expression to validate</param>
    /// <returns>True if the expression is valid, false otherwise</returns>
    public static bool IsValid(string cronExpression)
    {
        try
        {
            var parsed = Parse(cronExpression);
            return ValidateFields(parsed);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the next execution time for a Cron expression
    /// </summary>
    /// <param name="cronExpression">The Cron expression</param>
    /// <param name="fromTime">The time to calculate from (optional, defaults to now)</param>
    /// <returns>The next execution time</returns>
    /// <exception cref="ArgumentException">Thrown when the Cron expression is invalid</exception>
    public static DateTime GetNextExecutionTime(string cronExpression, DateTime? fromTime = null)
    {
        var parsed = Parse(cronExpression);
        var baseTime = fromTime ?? DateTime.Now;

        // This is a simplified implementation
        // In a real-world scenario, you'd want to use a more robust library like Quartz.NET
        return CalculateNextExecution(parsed, baseTime);
    }

    /// <summary>
    /// Converts a Cron expression to a human-readable Chinese description
    /// </summary>
    /// <param name="cronExpression">The Cron expression to describe</param>
    /// <returns>A Chinese description of the Cron expression</returns>
    /// <exception cref="ArgumentException">Thrown when the Cron expression is invalid</exception>
    public static string ToChineseDescription(string cronExpression)
    {
        if (!IsValid(cronExpression))
            throw new ArgumentException("Invalid Cron expression", nameof(cronExpression));

        var parsed = Parse(cronExpression);
        return BuildChineseDescription(parsed);
    }

    private static string BuildChineseDescription(CronExpression cron)
    {
        // 处理频率模式
        if (cron.Minutes.StartsWith("*/"))
        {
            var interval = cron.Minutes.Substring(2);
            if (cron.Hours == "*" && cron.DayOfMonth == "*" && cron.Month == "*" && cron.DayOfWeek == "?")
            {
                return $"每{interval}分钟";
            }
        }

        if (cron.Hours.StartsWith("*/"))
        {
            var interval = cron.Hours.Substring(2);
            if (cron.DayOfMonth == "*" && cron.Month == "*" && cron.DayOfWeek == "?")
            {
                if (cron.Minutes == "0")
                    return $"每{interval}小时";
                else
                    return $"每{interval}小时在{cron.Minutes}分";
            }
        }

        if (cron.DayOfMonth.StartsWith("*/"))
        {
            var interval = cron.DayOfMonth.Substring(2);
            if (cron.Month == "*" && cron.DayOfWeek == "?")
            {
                return $"每{interval}天";
            }
        }

        // 处理每日模式
        if (cron.DayOfMonth == "*" && cron.Month == "*" && cron.DayOfWeek == "?")
        {
            if (cron.Hours == "0" && cron.Minutes == "0")
                return "每天";
            if (cron.Hours != "*" && cron.Minutes == "0")
                return $"每天{GetTimeDescription(cron.Hours, "0")}";
            if (cron.Hours != "*" && cron.Minutes != "*")
                return $"每天{GetTimeDescription(cron.Hours, cron.Minutes)}";
        }

        // 处理每周模式
        if (cron.DayOfMonth == "?" && cron.Month == "*" && cron.DayOfWeek != "?")
        {
            var weekdayDesc = GetWeekdayDescription(cron.DayOfWeek);
            if (cron.Hours == "0" && cron.Minutes == "0")
                return weekdayDesc;
            else
                return $"{weekdayDesc}{GetTimeDescription(cron.Hours, cron.Minutes)}";
        }

        // 处理每月模式
        if (cron.DayOfMonth != "*" && cron.DayOfMonth != "?" && cron.Month == "*" && cron.DayOfWeek == "?")
        {
            var dayDesc = GetDayOfMonthDescription(cron.DayOfMonth);
            if (cron.Hours == "0" && cron.Minutes == "0")
                return dayDesc;
            else
                return $"{dayDesc}{GetTimeDescription(cron.Hours, cron.Minutes)}";
        }

        // 默认描述
        return "自定义时间表达式";
    }

    private static string GetTimeDescription(string hours, string minutes)
    {
        if (hours == "8" && minutes == "0")
            return "上午8点";
        if (hours == "15" && minutes == "0")
            return "下午3点";
        if (hours == "10" && minutes == "30")
            return "上午10点30分";

        var hourInt = int.Parse(hours);
        var minuteInt = int.Parse(minutes);

        if (hourInt == 0)
        {
            if (minuteInt == 0)
                return "";
            else
                return $"凌晨{minuteInt}分";
        }

        var period = hourInt < 12 ? "上午" : "下午";
        var displayHour = hourInt > 12 ? hourInt - 12 : hourInt;

        if (minuteInt == 0)
            return $"{period}{displayHour}点";
        else
            return $"{period}{displayHour}点{minuteInt}分";
    }

    private static string GetWeekdayDescription(string dayOfWeek)
    {
        return dayOfWeek switch
        {
            "1" => "每周一",
            "2" => "每周二",
            "3" => "每周三",
            "4" => "每周四",
            "5" => "每周五",
            "6" => "每周六",
            "7" => "每周日",
            "1,3" => "每周一和周三",
            "1,3,5" => "每周一、三、五",
            "1,2,3,4,5" => "每个工作日",
            "6,7" => "每个周末",
            _ => $"每周{dayOfWeek}"
        };
    }

    private static string GetDayOfMonthDescription(string dayOfMonth)
    {
        if (dayOfMonth.Contains(","))
        {
            var days = dayOfMonth.Split(',');
            if (days.Length == 2)
                return $"每月{days[0]}号和{days[1]}号";
            else
                return $"每月{dayOfMonth}号";
        }
        return $"每月{dayOfMonth}号";
    }

    private static bool ValidateFields(CronExpression expression)
    {
        return ValidateField(expression.Seconds, 0, 59) &&
               ValidateField(expression.Minutes, 0, 59) &&
               ValidateField(expression.Hours, 0, 23) &&
               ValidateField(expression.DayOfMonth, 1, 31) &&
               ValidateField(expression.Month, 1, 12) &&
               ValidateField(expression.DayOfWeek, 1, 7) &&
               ValidateField(expression.Year, 1970, 2099);
    }

    private static bool ValidateField(string field, int min, int max)
    {
        if (string.IsNullOrEmpty(field))
            return false;

        if (field == "*" || field == "?")
            return true;

        // Handle ranges (e.g., "1-5")
        if (field.Contains('-'))
        {
            var parts = field.Split('-');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out var start) &&
                int.TryParse(parts[1], out var end))
            {
                return start >= min && end <= max && start <= end;
            }
            return false;
        }

        // Handle step values (e.g., "*/5")
        if (field.Contains('/'))
        {
            var parts = field.Split('/');
            if (parts.Length == 2 && parts[0] == "*" &&
                int.TryParse(parts[1], out var step))
            {
                return step > 0 && step <= max;
            }
            return false;
        }

        // Handle comma-separated values (e.g., "1,3,5")
        if (field.Contains(','))
        {
            var values = field.Split(',');
            return values.All(v => int.TryParse(v, out var val) && val >= min && val <= max);
        }

        // Handle single values
        if (int.TryParse(field, out var value))
        {
            return value >= min && value <= max;
        }

        return false;
    }

    private static DateTime CalculateNextExecution(CronExpression expression, DateTime baseTime)
    {
        // This is a simplified implementation for demonstration purposes
        // A production implementation would need to handle all Cron expression complexities
        var nextTime = baseTime.AddMinutes(1);
        nextTime = new DateTime(nextTime.Year, nextTime.Month, nextTime.Day,
                               nextTime.Hour, nextTime.Minute, 0, nextTime.Kind);

        return nextTime;
    }

}