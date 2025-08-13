using FluentAssertions;
using Xunit;

namespace CronTools.Tests;

public class CronExtensionsTests
{
    [Theory]
    [InlineData(8, 30, 0, "0 30 8 * * ? *")]
    [InlineData(12, 0, 15, "15 0 12 * * ? *")]
    [InlineData(23, 59, 59, "59 59 23 * * ? *")]
    public void DailyAt_WithValidParameters_ShouldCreateCorrectExpression(int hour, int minute, int second, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.DailyAt(hour, minute, second).Build();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void DailyAt_WithDefaultSecond_ShouldUseZero()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.DailyAt(9, 30).Build();

        // Assert
        result.Should().Be("0 30 9 * * ? *");
    }

    [Theory]
    [InlineData(1, 9, 0, 0, "0 0 9 ? * 1 *")]
    [InlineData(5, 17, 30, 15, "15 30 17 ? * 5 *")]
    [InlineData(7, 10, 15, 30, "30 15 10 ? * 7 *")]
    public void WeeklyAt_WithValidParameters_ShouldCreateCorrectExpression(int dayOfWeek, int hour, int minute, int second, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.WeeklyAt(dayOfWeek, hour, minute, second).Build();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void WeeklyAt_WithDefaultSecond_ShouldUseZero()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.WeeklyAt(1, 9, 30).Build();

        // Assert
        result.Should().Be("0 30 9 ? * 1 *");
    }

    [Theory]
    [InlineData(15, 14, 30, 0, "0 30 14 15 * ? *")]
    [InlineData(1, 0, 0, 30, "30 0 0 1 * ? *")]
    [InlineData(31, 23, 59, 59, "59 59 23 31 * ? *")]
    public void MonthlyAt_WithValidParameters_ShouldCreateCorrectExpression(int dayOfMonth, int hour, int minute, int second, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.MonthlyAt(dayOfMonth, hour, minute, second).Build();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void MonthlyAt_WithDefaultSecond_ShouldUseZero()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.MonthlyAt(15, 10, 30).Build();

        // Assert
        result.Should().Be("0 30 10 15 * ? *");
    }

    [Fact]
    public void AtStartOfHour_ShouldCreateCorrectExpression()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.AtStartOfHour().Build();

        // Assert
        result.Should().Be("0 0 * * * ? *");
    }

    [Fact]
    public void AtMidnight_ShouldCreateCorrectExpression()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.AtMidnight().Build();

        // Assert
        result.Should().Be("0 0 0 * * ? *");
    }

    [Fact]
    public void AtNoon_ShouldCreateCorrectExpression()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.AtNoon().Build();

        // Assert
        result.Should().Be("0 0 12 * * ? *");
    }

    [Theory]
    [InlineData(0, 0, "0 0 0 1 * ? *")]
    [InlineData(9, 30, "0 30 9 1 * ? *")]
    [InlineData(23, 59, "0 59 23 1 * ? *")]
    public void OnFirstDayOfMonth_WithParameters_ShouldCreateCorrectExpression(int hour, int minute, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnFirstDayOfMonth(hour, minute).Build();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void OnFirstDayOfMonth_WithDefaults_ShouldUseZeroHourAndMinute()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnFirstDayOfMonth().Build();

        // Assert
        result.Should().Be("0 0 0 1 * ? *");
    }

    [Theory]
    [InlineData(0, 0, "0 0 0 31 * ? *")]
    [InlineData(9, 30, "0 30 9 31 * ? *")]
    [InlineData(23, 59, "0 59 23 31 * ? *")]
    public void OnLastDayOfMonth_WithParameters_ShouldCreateCorrectExpression(int hour, int minute, string expected)
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnLastDayOfMonth(hour, minute).Build();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void OnLastDayOfMonth_WithDefaults_ShouldUseZeroHourAndMinute()
    {
        // Arrange
        var builder = CronBuilder.Create();

        // Act
        var result = builder.OnLastDayOfMonth().Build();

        // Assert
        result.Should().Be("0 0 0 31 * ? *");
    }

    [Fact]
    public void IsValid_WithValidBuilder_ShouldReturnTrue()
    {
        // Arrange
        var builder = CronBuilder.Create().DailyAt(9, 30);

        // Act
        var result = builder.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetNextExecutionTime_WithValidBuilder_ShouldReturnFutureTime()
    {
        // Arrange
        var builder = CronBuilder.Create().DailyAt(12, 0);
        var fromTime = new DateTime(2023, 1, 1, 10, 0, 0);

        // Act
        var result = builder.GetNextExecutionTime(fromTime);

        // Assert
        result.Should().BeAfter(fromTime);
    }

    [Fact]
    public void GetNextExecutionTime_WithoutFromTime_ShouldUseCurrentTime()
    {
        // Arrange
        var builder = CronBuilder.Create().DailyAt(12, 0);
        var beforeCall = DateTime.Now;

        // Act
        var result = builder.GetNextExecutionTime();

        // Assert
        result.Should().BeAfter(beforeCall);
    }
}

public class CronPresetsTests
{
    [Fact]
    public void EveryMinute_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.EveryMinute;

        // Assert
        result.Should().Be("0 * * * * ? *");
    }

    [Fact]
    public void Every5Minutes_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.Every5Minutes;

        // Assert
        result.Should().Be("0 */5 * * * ? *");
    }

    [Fact]
    public void Every15Minutes_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.Every15Minutes;

        // Assert
        result.Should().Be("0 */15 * * * ? *");
    }

    [Fact]
    public void Every30Minutes_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.Every30Minutes;

        // Assert
        result.Should().Be("0 */30 * * * ? *");
    }

    [Fact]
    public void EveryHour_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.EveryHour;

        // Assert
        result.Should().Be("0 0 * * * ? *");
    }

    [Fact]
    public void Every2Hours_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.Every2Hours;

        // Assert
        result.Should().Be("0 0 */2 * * ? *");
    }

    [Fact]
    public void Every6Hours_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.Every6Hours;

        // Assert
        result.Should().Be("0 0 */6 * * ? *");
    }

    [Fact]
    public void Every12Hours_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.Every12Hours;

        // Assert
        result.Should().Be("0 0 */12 * * ? *");
    }

    [Fact]
    public void Daily_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.Daily;

        // Assert
        result.Should().Be("0 0 0 * * ? *");
    }

    [Fact]
    public void WeeklyMonday_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.WeeklyMonday;

        // Assert
        result.Should().Be("0 0 9 ? * 1 *");
    }

    [Fact]
    public void Monthly_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.Monthly;

        // Assert
        result.Should().Be("0 0 0 1 * ? *");
    }

    [Fact]
    public void WeekdaysAt9AM_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.WeekdaysAt9AM;

        // Assert
        result.Should().Be("0 0 9 ? * 1,2,3,4,5 *");
    }

    [Fact]
    public void EveryNHours_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronBuilder.Create().EveryNHours(6).Build();

        // Assert
        result.Should().Be("0 0 */6 * * ? *");
    }

    [Fact]
    public void OnWeekends_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronBuilder.Create().OnWeekends().Build();

        // Assert
        result.Should().Be("0 0 0 ? * 6,7 *");
    }

    [Fact]
    public void AtMidnight_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronBuilder.Create().AtMidnight().Build();

        // Assert
        result.Should().Be("0 0 0 * * ? *");
    }

    [Fact]
    public void AtNoon_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronBuilder.Create().AtNoon().Build();

        // Assert
        result.Should().Be("0 0 12 * * ? *");
    }

    [Fact]
    public void OnLastDayOfMonth_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronBuilder.Create().OnLastDayOfMonth().Build();

        // Assert
        result.Should().Be("0 0 0 31 * ? *");
    }

    [Fact]
    public void OnFirstDayOfMonth_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronBuilder.Create().OnFirstDayOfMonth().Build();

        // Assert
        result.Should().Be("0 0 0 1 * ? *");
    }

    [Fact]
    public void WeekendsAt10AM_ShouldReturnCorrectExpression()
    {
        // Act
        var result = CronPresets.WeekendsAt10AM;

        // Assert
        result.Should().Be("0 0 10 ? * 6,7 *");
    }

    [Fact]
    public void AllPresets_ShouldBeValidCronExpressions()
    {
        // Arrange
        var presets = new[]
        {
            CronPresets.EveryMinute,
            CronPresets.Every5Minutes,
            CronPresets.Every15Minutes,
            CronPresets.Every30Minutes,
            CronPresets.EveryHour,
            CronPresets.Every2Hours,
            CronPresets.Every6Hours,
            CronPresets.Every12Hours,
            CronPresets.Daily,
            CronPresets.WeeklyMonday,
            CronPresets.Monthly,
            CronPresets.WeekdaysAt9AM,
            CronPresets.WeekendsAt10AM
        };

        // Act & Assert
        foreach (var preset in presets)
        {
            CronParser.IsValid(preset).Should().BeTrue($"Preset '{preset}' should be valid");
        }
    }
}
