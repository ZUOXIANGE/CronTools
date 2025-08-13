using FluentAssertions;
using Xunit;

namespace CronTools.Tests;

public class CronBuilderTests
{
    [Fact]
    public void Create_ShouldReturnNewInstance()
    {
        // Act
        var builder = CronBuilder.Create();

        // Assert
        builder.Should().NotBeNull();
        builder.Should().BeOfType<CronBuilder>();
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(30, "30")]
    [InlineData(59, "59")]
    public void AtSecond_WithValidValue_ShouldSetSeconds(int second, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.AtSecond(second).Build();

        // Assert
        result.Should().StartWith(expected);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(60)]
    [InlineData(100)]
    public void AtSecond_WithInvalidValue_ShouldThrowArgumentOutOfRangeException(int second)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.AtSecond(second))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("second");
    }

    [Fact]
    public void AtSeconds_WithMultipleValues_ShouldSetCorrectExpression()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.AtSeconds(0, 15, 30, 45).Build();

        // Assert
        result.Should().StartWith("0,15,30,45");
    }

    [Fact]
    public void AtSeconds_WithDuplicateValues_ShouldRemoveDuplicates()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.AtSeconds(0, 15, 0, 30, 15).Build();

        // Assert
        result.Should().StartWith("0,15,30");
    }

    [Theory]
    [InlineData(new[] { -1, 0 })]
    [InlineData(new[] { 0, 60 })]
    [InlineData(new[] { 30, 100 })]
    public void AtSeconds_WithInvalidValues_ShouldThrowArgumentOutOfRangeException(int[] seconds)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.AtSeconds(seconds))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("seconds");
    }

    [Fact]
    public void ComplexCombination_ShouldGenerateCorrectExpression()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder
            .AtSeconds(0, 30)
            .AtMinutes(15, 45)
            .AtHours(9, 17)
            .OnDays(1, 15)
            .InMonths(3, 6, 9, 12)
            .Build();

        // Assert
        result.Should().Be("0,30 15,45 9,17 1,15 3,6,9,12 ? *");
    }

    [Fact]
    public void WeekdayAndDayOfMonth_ShouldPrioritizeWeekday()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder
            .OnDay(15)
            .OnMonday()
            .Build();

        // Assert
        result.Should().EndWith("? * 1 *");
    }

    [Fact]
    public void RangeOperations_ShouldGenerateCorrectExpression()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder
            .OnDaysBetween(10, 20)
            .InMonthsBetween(4, 8)
            .InYearsBetween(2024, 2026)
            .Build();

        // Assert
        result.Should().Be("0 0 0 10-20 4-8 ? 2024-2026");
    }

    [Fact]
    public void AtSeconds_WithMultipleValues_ShouldSetSecondsCommaSeparated()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.AtSeconds(0, 15, 30, 45).Build();

        // Assert
        result.Should().StartWith("0,15,30,45");
    }

    [Fact]
    public void AtSeconds_WithDuplicateValues_ShouldRemoveDuplicatesAndSort()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.AtSeconds(30, 0, 15, 30, 45, 0).Build();

        // Assert
        result.Should().StartWith("0,15,30,45");
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(30, "30")]
    [InlineData(59, "59")]
    public void AtMinute_WithValidValue_ShouldSetMinutes(int minute, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.AtMinute(minute).Build();

        // Assert
        var parts = result.Split(' ');
        parts[1].Should().Be(expected);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(60)]
    public void AtMinute_WithInvalidValue_ShouldThrowArgumentOutOfRangeException(int minute)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.AtMinute(minute))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("minute");
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(12, "12")]
    [InlineData(23, "23")]
    public void AtHour_WithValidValue_ShouldSetHours(int hour, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.AtHour(hour).Build();

        // Assert
        var parts = result.Split(' ');
        parts[2].Should().Be(expected);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(24)]
    public void AtHour_WithInvalidValue_ShouldThrowArgumentOutOfRangeException(int hour)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.AtHour(hour))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("hour");
    }

    [Theory]
    [InlineData(1, "1")]
    [InlineData(15, "15")]
    [InlineData(31, "31")]
    public void OnDay_WithValidValue_ShouldSetDayOfMonth(int day, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnDay(day).Build();

        // Assert
        var parts = result.Split(' ');
        parts[3].Should().Be(expected);
        parts[5].Should().Be("?"); // Day of week should be ?
    }

    [Theory]
    [InlineData(0)]
    [InlineData(32)]
    public void OnDay_WithInvalidValue_ShouldThrowArgumentOutOfRangeException(int day)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.OnDay(day))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("day");
    }

    [Fact]
    public void OnDaysBetween_WithValidRange_ShouldSetDayOfMonthRange()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnDaysBetween(10, 20).Build();

        // Assert
        var parts = result.Split(' ');
        parts[3].Should().Be("10-20");
    }

    [Fact]
    public void OnDaysBetween_WithStartGreaterThanEnd_ShouldThrowArgumentException()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.OnDaysBetween(20, 10))
            .Should().Throw<ArgumentException>()
            .WithMessage("Start day cannot be greater than end day");
    }

    [Theory]
    [InlineData(1, "1")]
    [InlineData(6, "6")]
    [InlineData(12, "12")]
    public void InMonth_WithValidValue_ShouldSetMonth(int month, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.InMonth(month).Build();

        // Assert
        var parts = result.Split(' ');
        parts[4].Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void InMonth_WithInvalidValue_ShouldThrowArgumentOutOfRangeException(int month)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.InMonth(month))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("month");
    }

    [Theory]
    [InlineData(1970, "1970")]
    [InlineData(2023, "2023")]
    [InlineData(2099, "2099")]
    public void InYear_WithValidValue_ShouldSetYear(int year, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.InYear(year).Build();

        // Assert
        var parts = result.Split(' ');
        parts[6].Should().Be(expected);
    }

    [Theory]
    [InlineData(1969)]
    [InlineData(2100)]
    public void InYear_WithInvalidValue_ShouldThrowArgumentOutOfRangeException(int year)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.InYear(year))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("year");
    }

    [Theory]
    [InlineData(1, "1")]
    [InlineData(4, "4")]
    [InlineData(7, "7")]
    public void OnWeekday_WithValidValue_ShouldSetDayOfWeek(int day, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnWeekday(day).Build();

        // Assert
        var parts = result.Split(' ');
        parts[5].Should().Be(expected);
        parts[3].Should().Be("?"); // Day of month should be ?
    }

    [Theory]
    [InlineData(0)]
    [InlineData(8)]
    public void OnWeekday_WithInvalidValue_ShouldThrowArgumentOutOfRangeException(int day)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.OnWeekday(day))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("day");
    }

    [Fact]
    public void OnMonday_ShouldSetDayOfWeekTo1()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnMonday().Build();

        // Assert
        var parts = result.Split(' ');
        parts[5].Should().Be("1");
    }

    [Fact]
    public void OnTuesday_ShouldSetDayOfWeekTo2()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnTuesday().Build();

        // Assert
        var parts = result.Split(' ');
        parts[5].Should().Be("2");
    }

    [Fact]
    public void OnWednesday_ShouldSetDayOfWeekTo3()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnWednesday().Build();

        // Assert
        var parts = result.Split(' ');
        parts[5].Should().Be("3");
    }

    [Fact]
    public void OnThursday_ShouldSetDayOfWeekTo4()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnThursday().Build();

        // Assert
        var parts = result.Split(' ');
        parts[5].Should().Be("4");
    }

    [Fact]
    public void OnFriday_ShouldSetDayOfWeekTo5()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnFriday().Build();

        // Assert
        var parts = result.Split(' ');
        parts[5].Should().Be("5");
    }

    [Fact]
    public void OnSaturday_ShouldSetDayOfWeekTo6()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnSaturday().Build();

        // Assert
        var parts = result.Split(' ');
        parts[5].Should().Be("6");
    }

    [Fact]
    public void OnSunday_ShouldSetDayOfWeekTo7()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnSunday().Build();

        // Assert
        var parts = result.Split(' ');
        parts[5].Should().Be("7");
    }

    [Fact]
    public void OnWeekdays_ShouldSetDayOfWeekToWeekdays()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnWeekdays().Build();

        // Assert
        var parts = result.Split(' ');
        parts[5].Should().Be("1,2,3,4,5");
    }

    [Fact]
    public void OnWeekends_ShouldSetDayOfWeekToWeekends()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnWeekends().Build();

        // Assert
        var parts = result.Split(' ');
        parts[5].Should().Be("6,7");
    }

    [Fact]
    public void Daily_ShouldSetDayOfMonthToAsteriskAndDayOfWeekToQuestion()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.Daily().Build();

        // Assert
        var parts = result.Split(' ');
        parts[3].Should().Be("*"); // Day of month
        parts[5].Should().Be("?"); // Day of week
    }

    [Fact]
    public void Hourly_WithoutParameter_ShouldSetMinutesToZero()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.Hourly().Build();

        // Assert
        var parts = result.Split(' ');
        parts[1].Should().Be("0"); // Minutes
        parts[2].Should().Be("*"); // Hours
    }

    [Fact]
    public void Hourly_WithParameter_ShouldSetMinutesToSpecifiedValue()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.Hourly(15).Build();

        // Assert
        var parts = result.Split(' ');
        parts[1].Should().Be("15"); // Minutes
        parts[2].Should().Be("*"); // Hours
    }

    [Fact]
    public void EveryMinute_ShouldSetMinutesToAsterisk()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.EveryMinute().Build();

        // Assert
        var parts = result.Split(' ');
        parts[1].Should().Be("*"); // Minutes
    }

    [Theory]
    [InlineData(5, "*/5")]
    [InlineData(10, "*/10")]
    [InlineData(15, "*/15")]
    public void EveryNMinutes_WithValidInterval_ShouldSetMinutesWithInterval(int interval, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.EveryNMinutes(interval).Build();

        // Assert
        var parts = result.Split(' ');
        parts[1].Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(60)]
    public void EveryNMinutes_WithInvalidInterval_ShouldThrowArgumentOutOfRangeException(int interval)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.EveryNMinutes(interval))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("interval");
    }

    [Theory]
    [InlineData(2, "*/2")]
    [InlineData(6, "*/6")]
    [InlineData(12, "*/12")]
    public void EveryNHours_WithValidInterval_ShouldSetHoursWithInterval(int interval, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.EveryNHours(interval).Build();

        // Assert
        var parts = result.Split(' ');
        parts[2].Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(24)]
    public void EveryNHours_WithInvalidInterval_ShouldThrowArgumentOutOfRangeException(int interval)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.EveryNHours(interval))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("interval");
    }

    [Theory]
    [InlineData(2, "*/2")]
    [InlineData(7, "*/7")]
    [InlineData(15, "*/15")]
    public void EveryNDays_WithValidInterval_ShouldSetDayOfMonthWithInterval(int interval, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.EveryNDays(interval).Build();

        // Assert
        var parts = result.Split(' ');
        parts[3].Should().Be(expected);
        parts[5].Should().Be("?"); // Day of week should be ?
    }

    [Theory]
    [InlineData(0)]
    [InlineData(32)]
    public void EveryNDays_WithInvalidInterval_ShouldThrowArgumentOutOfRangeException(int interval)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act & Assert
        builder.Invoking(b => b.EveryNDays(interval))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("interval");
    }

    [Fact]
    public void Build_WithDayOfMonthAndDayOfWeekSet_ShouldPrioritizeDayOfWeek()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnDay(15).OnMonday().Build();

        // Assert
        var parts = result.Split(' ');
        parts[3].Should().Be("?"); // Day of month should be ?
        parts[5].Should().Be("1"); // Day of week should be 1 (Monday)
    }

    [Fact]
    public void Build_WithNeitherDayOfMonthNorDayOfWeekSet_ShouldUseDefaults()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.Build();

        // Assert
        var parts = result.Split(' ');
        parts[3].Should().Be("*"); // Day of month should be *
        parts[5].Should().Be("?"); // Day of week should be ?
    }

    [Fact]
    public void ComplexExpression_ShouldBuildCorrectly()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder
            .AtSecond(30)
            .AtMinutes(0, 15, 30, 45)
            .AtHours(9, 12, 15, 18)
            .OnDays(5, 15, 25)
            .InMonths(3, 6, 9, 12)
            .Build();

        // Assert
        result.Should().Be("30 0,15,30,45 9,12,15,18 5,15,25 3,6,9,12 ? *");
    }

    [Fact]
    public void MultipleWeekdaySelection_ShouldCombineCorrectly()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder
            .OnMonday()
            .OnWednesday()
            .OnFriday()
            .OnSunday()
            .AtHour(15)
            .AtMinute(0)
            .Build();

        // Assert
        var parts = result.Split(' ');
        parts[5].Should().Be("1,3,5,7");
    }

    [Fact]
    public void GetChineseDescription_WithDailySchedule_ShouldReturnCorrectDescription()
    {
        // Arrange & Act
        var description = CronBuilder.Create().GetChineseDescription();

        // Assert
        description.Should().Be("每天");
    }

    [Fact]
    public void GetChineseDescription_WithSpecificTime_ShouldReturnCorrectDescription()
    {
        // Arrange & Act
        var description = CronBuilder.Create()
            .AtHour(8)
            .GetChineseDescription();

        // Assert
        description.Should().Be("每天上午8点");
    }

    [Fact]
    public void GetChineseDescription_WithWeekday_ShouldReturnCorrectDescription()
    {
        // Arrange & Act
        var description = CronBuilder.Create()
            .OnMonday()
            .GetChineseDescription();

        // Assert
        description.Should().Be("每周一");
    }

    [Fact]
    public void GetChineseDescription_WithComplexSchedule_ShouldReturnCorrectDescription()
    {
        // Arrange & Act
        var description = CronBuilder.Create()
            .AtHour(15)
            .OnWeekdays(1, 3)
            .GetChineseDescription();

        // Assert
        description.Should().Be("每周一和周三下午3点");
    }

    [Fact]
    public void GetChineseDescription_WithMonthlySchedule_ShouldReturnCorrectDescription()
    {
        // Arrange & Act
        var description = CronBuilder.Create()
            .OnDay(15)
            .GetChineseDescription();

        // Assert
        description.Should().Be("每月15号");
    }
}