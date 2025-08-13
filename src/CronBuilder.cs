namespace CronTools;

/// <summary>
/// A fluent API for building Cron expressions
/// </summary>
public class CronBuilder
{
    private string _seconds = "0";
    private string _minutes = "0";
    private string _hours = "0";
    private string _dayOfMonth = "*";
    private string _month = "*";
    private string _dayOfWeek = "?";
    private string _year = "*";

    private bool _dayOfMonthSet;
    private bool _dayOfWeekSet;

    /// <summary>
    /// Private constructor to enforce factory pattern
    /// </summary>
    private CronBuilder() { }

    /// <summary>
    /// Creates a new CronBuilder instance
    /// </summary>
    /// <returns>A new CronBuilder instance</returns>
    public static CronBuilder Create()
    {
        return new CronBuilder();
    }

    /// <summary>
    /// Sets the second field (0-59)
    /// </summary>
    /// <param name="second">The second value (0-59)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when second is not between 0 and 59</exception>
    public CronBuilder AtSecond(int second)
    {
        ValidateRange(second, 0, 59, nameof(second));
        _seconds = second.ToString();
        return this;
    }

    /// <summary>
    /// Sets multiple second values
    /// </summary>
    /// <param name="seconds">Array of second values (0-59)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any second value is not between 0 and 59</exception>
    public CronBuilder AtSeconds(params int[] seconds)
    {
        ValidateValues(seconds, 0, 59, nameof(seconds));
        _seconds = string.Join(",", seconds.Distinct().OrderBy(s => s));
        return this;
    }

    /// <summary>
    /// Sets the minute field (0-59)
    /// </summary>
    /// <param name="minute">The minute value (0-59)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minute is not between 0 and 59</exception>
    public CronBuilder AtMinute(int minute)
    {
        ValidateRange(minute, 0, 59, nameof(minute));
        _minutes = minute.ToString();
        return this;
    }

    /// <summary>
    /// Sets multiple minute values
    /// </summary>
    /// <param name="minutes">Array of minute values (0-59)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any minute value is not between 0 and 59</exception>
    public CronBuilder AtMinutes(params int[] minutes)
    {
        ValidateValues(minutes, 0, 59, nameof(minutes));
        _minutes = string.Join(",", minutes.Distinct().OrderBy(m => m));
        return this;
    }

    /// <summary>
    /// Sets the hour field (0-23)
    /// </summary>
    /// <param name="hour">The hour value (0-23)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when hour is not between 0 and 23</exception>
    public CronBuilder AtHour(int hour)
    {
        ValidateRange(hour, 0, 23, nameof(hour));
        _hours = hour.ToString();
        return this;
    }

    /// <summary>
    /// Sets multiple hour values
    /// </summary>
    /// <param name="hours">Array of hour values (0-23)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any hour value is not between 0 and 23</exception>
    public CronBuilder AtHours(params int[] hours)
    {
        ValidateValues(hours, 0, 23, nameof(hours));
        _hours = string.Join(",", hours.Distinct().OrderBy(h => h));
        return this;
    }

    /// <summary>
    /// Sets the day of month field (1-31)
    /// </summary>
    /// <param name="day">The day value (1-31)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when day is not between 1 and 31</exception>
    public CronBuilder OnDay(int day)
    {
        ValidateRange(day, 1, 31, nameof(day));
        _dayOfMonth = day.ToString();
        _dayOfMonthSet = true;
        ResetDayOfWeekIfNeeded();
        return this;
    }

    /// <summary>
    /// Sets multiple day of month values
    /// </summary>
    /// <param name="days">Array of day values (1-31)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any day value is not between 1 and 31</exception>
    public CronBuilder OnDays(params int[] days)
    {
        ValidateValues(days, 1, 31, nameof(days));
        _dayOfMonth = string.Join(",", days.Distinct().OrderBy(d => d));
        _dayOfMonthSet = true;
        ResetDayOfWeekIfNeeded();
        return this;
    }

    /// <summary>
    /// Sets a range of days for the day of month field
    /// </summary>
    /// <param name="start">Start day (1-31)</param>
    /// <param name="end">End day (1-31)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when start or end is not between 1 and 31</exception>
    /// <exception cref="ArgumentException">Thrown when start is greater than end</exception>
    public CronBuilder OnDaysBetween(int start, int end)
    {
        ValidateRange(start, 1, 31, nameof(start));
        ValidateRange(end, 1, 31, nameof(end));
        if (start > end) throw new ArgumentException("Start day cannot be greater than end day");
        _dayOfMonth = $"{start}-{end}";
        _dayOfMonthSet = true;
        ResetDayOfWeekIfNeeded();
        return this;
    }

    /// <summary>
    /// Sets the month field (1-12)
    /// </summary>
    /// <param name="month">The month value (1-12)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when month is not between 1 and 12</exception>
    public CronBuilder InMonth(int month)
    {
        ValidateRange(month, 1, 12, nameof(month));
        _month = month.ToString();
        return this;
    }

    /// <summary>
    /// Sets multiple month values
    /// </summary>
    /// <param name="months">Array of month values (1-12)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any month value is not between 1 and 12</exception>
    public CronBuilder InMonths(params int[] months)
    {
        ValidateValues(months, 1, 12, nameof(months));
        _month = string.Join(",", months.Distinct().OrderBy(m => m));
        return this;
    }

    /// <summary>
    /// Sets a range of months
    /// </summary>
    /// <param name="start">Start month (1-12)</param>
    /// <param name="end">End month (1-12)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when start or end is not between 1 and 12</exception>
    /// <exception cref="ArgumentException">Thrown when start is greater than end</exception>
    public CronBuilder InMonthsBetween(int start, int end)
    {
        ValidateRange(start, 1, 12, nameof(start));
        ValidateRange(end, 1, 12, nameof(end));
        if (start > end) throw new ArgumentException("Start month cannot be greater than end month");
        _month = $"{start}-{end}";
        return this;
    }

    /// <summary>
    /// Sets the year field
    /// </summary>
    /// <param name="year">The year value (1970-2099)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when year is not between 1970 and 2099</exception>
    public CronBuilder InYear(int year)
    {
        ValidateRange(year, 1970, 2099, nameof(year));
        _year = year.ToString();
        return this;
    }

    /// <summary>
    /// Sets multiple year values
    /// </summary>
    /// <param name="years">Array of year values (1970-2099)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any year value is not between 1970 and 2099</exception>
    public CronBuilder InYears(params int[] years)
    {
        ValidateValues(years, 1970, 2099, nameof(years));
        _year = string.Join(",", years.Distinct().OrderBy(y => y));
        return this;
    }

    /// <summary>
    /// Sets a range of years
    /// </summary>
    /// <param name="start">Start year (1970-2099)</param>
    /// <param name="end">End year (1970-2099)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when start or end is not between 1970 and 2099</exception>
    /// <exception cref="ArgumentException">Thrown when start is greater than end</exception>
    public CronBuilder InYearsBetween(int start, int end)
    {
        ValidateRange(start, 1970, 2099, nameof(start));
        ValidateRange(end, 1970, 2099, nameof(end));
        if (start > end) throw new ArgumentException("Start year cannot be greater than end year");
        _year = $"{start}-{end}";
        return this;
    }

    /// <summary>
    /// Sets the day of week field (1-7, where 1=Monday, 7=Sunday)
    /// </summary>
    /// <param name="day">The day of week value (1-7)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when day is not between 1 and 7</exception>
    public CronBuilder OnWeekday(int day)
    {
        ValidateRange(day, 1, 7, nameof(day));
        _dayOfWeek = day.ToString();
        _dayOfWeekSet = true;
        ResetDayOfMonthIfNeeded();
        return this;
    }

    /// <summary>
    /// Sets multiple day of week values
    /// </summary>
    /// <param name="days">Array of day of week values (1-7)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any day value is not between 1 and 7</exception>
    public CronBuilder OnWeekdays(params int[] days)
    {
        ValidateValues(days, 1, 7, nameof(days));
        _dayOfWeek = string.Join(",", days.Distinct().OrderBy(d => d));
        _dayOfWeekSet = true;
        ResetDayOfMonthIfNeeded();
        return this;
    }

    /// <summary>
    /// Sets execution on Monday
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder OnMonday() => SetWeekday(1);

    /// <summary>
    /// Sets execution on Tuesday
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder OnTuesday() => SetWeekday(2);

    /// <summary>
    /// Sets execution on Wednesday
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder OnWednesday() => SetWeekday(3);

    /// <summary>
    /// Sets execution on Thursday
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder OnThursday() => SetWeekday(4);

    /// <summary>
    /// Sets execution on Friday
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder OnFriday() => SetWeekday(5);

    /// <summary>
    /// Sets execution on Saturday
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder OnSaturday() => SetWeekday(6);

    /// <summary>
    /// Sets execution on Sunday
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder OnSunday() => SetWeekday(7);

    /// <summary>
    /// Sets execution on weekdays (Monday through Friday)
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder OnWeekdays() => OnWeekdays(1, 2, 3, 4, 5);

    /// <summary>
    /// Sets execution on weekends (Saturday and Sunday)
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder OnWeekends() => OnWeekdays(6, 7);

    /// <summary>
    /// Sets daily execution
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder Daily()
    {
        _dayOfMonth = "*";
        _dayOfWeek = "?";
        _dayOfMonthSet = true;
        _dayOfWeekSet = false;
        return this;
    }

    /// <summary>
    /// Sets hourly execution at minute 0
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder Hourly() => Hourly(0);

    /// <summary>
    /// Sets hourly execution at the specified minute
    /// </summary>
    /// <param name="atMinute">The minute to execute at (0-59)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when atMinute is not between 0 and 59</exception>
    public CronBuilder Hourly(int atMinute)
    {
        ValidateRange(atMinute, 0, 59, nameof(atMinute));
        _minutes = atMinute.ToString();
        _hours = "*";
        return this;
    }

    /// <summary>
    /// Sets execution every minute
    /// </summary>
    /// <returns>The CronBuilder instance for method chaining</returns>
    public CronBuilder EveryMinute()
    {
        _minutes = "*";
        _hours = "*";
        return this;
    }

    /// <summary>
    /// Sets execution every N minutes
    /// </summary>
    /// <param name="interval">The minute interval (1-59)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when interval is not between 1 and 59</exception>
    public CronBuilder EveryNMinutes(int interval)
    {
        ValidateRange(interval, 1, 59, nameof(interval));
        _minutes = $"*/{interval}";
        _hours = "*";
        return this;
    }

    /// <summary>
    /// Sets execution every N hours
    /// </summary>
    /// <param name="interval">The hour interval (1-23)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when interval is not between 1 and 23</exception>
    public CronBuilder EveryNHours(int interval)
    {
        ValidateRange(interval, 1, 23, nameof(interval));
        _hours = $"*/{interval}";
        return this;
    }

    /// <summary>
    /// Sets execution every N days
    /// </summary>
    /// <param name="interval">The day interval (1-31)</param>
    /// <returns>The CronBuilder instance for method chaining</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when interval is not between 1 and 31</exception>
    public CronBuilder EveryNDays(int interval)
    {
        ValidateRange(interval, 1, 31, nameof(interval));
        _dayOfMonth = $"*/{interval}";
        _dayOfMonthSet = true;
        ResetDayOfWeekIfNeeded();
        return this;
    }

    /// <summary>
    /// Builds the Cron expression
    /// </summary>
    /// <returns>A valid Cron expression string</returns>
    public string Build()
    {
        // Handle mutual exclusion between day of month and day of week
        if (_dayOfMonthSet && _dayOfWeekSet)
        {
            // If both are set, prioritize day of week
            _dayOfMonth = "?";
        }
        else if (!_dayOfMonthSet && !_dayOfWeekSet)
        {
            // If neither is set, use daily default
            _dayOfMonth = "*";
            _dayOfWeek = "?";
        }
        else if (_dayOfWeekSet)
        {
            // If only day of week is set, set day of month to ?
            _dayOfMonth = "?";
        }
        else
        {
            // If only day of month is set, set day of week to ?
            _dayOfWeek = "?";
        }

        return $"{_seconds} {_minutes} {_hours} {_dayOfMonth} {_month} {_dayOfWeek} {_year}";
    }

    /// <summary>
    /// 获取当前构建器的中文描述
    /// </summary>
    /// <returns>Cron表达式的中文描述</returns>
    public string GetChineseDescription()
    {
        var cronExpression = Build();
        return CronParser.ToChineseDescription(cronExpression);
    }

    private CronBuilder SetWeekday(int day)
    {
        if (string.IsNullOrEmpty(_dayOfWeek) || _dayOfWeek == "?")
        {
            _dayOfWeek = day.ToString();
        }
        else
        {
            var days = _dayOfWeek.Split(',')
                .Select(int.Parse)
                .ToList();

            if (!days.Contains(day))
            {
                days.Add(day);
                _dayOfWeek = string.Join(",", days.OrderBy(d => d));
            }
        }

        _dayOfWeekSet = true;
        ResetDayOfMonthIfNeeded();
        return this;
    }

    private void ResetDayOfWeekIfNeeded()
    {
        if (_dayOfMonthSet)
        {
            _dayOfWeek = "?";
            _dayOfWeekSet = false;
        }
    }

    private void ResetDayOfMonthIfNeeded()
    {
        if (_dayOfWeekSet)
        {
            _dayOfMonth = "?";
            _dayOfMonthSet = false;
        }
    }

    private static void ValidateRange(int value, int min, int max, string parameterName)
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} must be between {min} and {max}");
        }
    }

    private static void ValidateValues(IEnumerable<int> values, int min, int max, string parameterName)
    {
        foreach (var value in values)
        {
            ValidateRange(value, min, max, parameterName);
        }
    }
}