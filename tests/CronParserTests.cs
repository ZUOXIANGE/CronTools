using FluentAssertions;
using Xunit;
using CronTools;

namespace CronTools.Tests;

public class CronParserTests
{
    [Theory]
    [InlineData("0 0 12 * * ? *", "0", "0", "12", "*", "*", "?", "*")]
    [InlineData("30 15 10 ? * 1-5 2023", "30", "15", "10", "?", "*", "1-5", "2023")]
    [InlineData("0 */5 * * * ? *", "0", "*/5", "*", "*", "*", "?", "*")]
    public void Parse_WithValidExpression_ShouldReturnCorrectComponents(string expression, 
        string expectedSeconds, string expectedMinutes, string expectedHours, 
        string expectedDayOfMonth, string expectedMonth, string expectedDayOfWeek, string expectedYear)
    {
        // Act
        var result = CronParser.Parse(expression);

        // Assert
        result.Should().NotBeNull();
        result.Seconds.Should().Be(expectedSeconds);
        result.Minutes.Should().Be(expectedMinutes);
        result.Hours.Should().Be(expectedHours);
        result.DayOfMonth.Should().Be(expectedDayOfMonth);
        result.Month.Should().Be(expectedMonth);
        result.DayOfWeek.Should().Be(expectedDayOfWeek);
        result.Year.Should().Be(expectedYear);
    }

    [Theory]
    [InlineData("0 0 12 * * ?", "0", "0", "12", "*", "*", "?", "*")] // 6 fields, year defaults to *
    public void Parse_WithSixFields_ShouldDefaultYearToAsterisk(string expression,
        string expectedSeconds, string expectedMinutes, string expectedHours,
        string expectedDayOfMonth, string expectedMonth, string expectedDayOfWeek, string expectedYear)
    {
        // Act
        var result = CronParser.Parse(expression);

        // Assert
        result.Should().NotBeNull();
        result.Seconds.Should().Be(expectedSeconds);
        result.Minutes.Should().Be(expectedMinutes);
        result.Hours.Should().Be(expectedHours);
        result.DayOfMonth.Should().Be(expectedDayOfMonth);
        result.Month.Should().Be(expectedMonth);
        result.DayOfWeek.Should().Be(expectedDayOfWeek);
        result.Year.Should().Be(expectedYear);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_WithNullOrEmptyExpression_ShouldThrowArgumentException(string? expression)
    {
        // Act & Assert
        FluentActions.Invoking(() => CronParser.Parse(expression!))
            .Should().Throw<ArgumentException>()
            .WithParameterName("cronExpression")
            .WithMessage("Cron expression cannot be null or empty*");
    }

    [Fact]
    public void Parse_WithComplexValidExpression_ShouldParseCorrectly()
    {
        // Arrange
        var expression = "30 15,45 9,17 1,15 3,6,9,12 ? 2024";

        // Act
        var result = CronParser.Parse(expression);

        // Assert
        result.Seconds.Should().Be("30");
        result.Minutes.Should().Be("15,45");
        result.Hours.Should().Be("9,17");
        result.DayOfMonth.Should().Be("1,15");
        result.Month.Should().Be("3,6,9,12");
        result.DayOfWeek.Should().Be("?");
        result.Year.Should().Be("2024");
    }

    [Fact]
    public void Parse_WithRangeExpression_ShouldParseCorrectly()
    {
        // Arrange
        var expression = "0 0 9-17 10-20 4-8 ? 2024-2026";

        // Act
        var result = CronParser.Parse(expression);

        // Assert
        result.Hours.Should().Be("9-17");
        result.DayOfMonth.Should().Be("10-20");
        result.Month.Should().Be("4-8");
        result.Year.Should().Be("2024-2026");
    }

    [Fact]
    public void Parse_WithStepValues_ShouldParseCorrectly()
    {
        // Arrange
        var expression = "0 */15 */2 * * ? *";

        // Act
        var result = CronParser.Parse(expression);

        // Assert
        result.Minutes.Should().Be("*/15");
        result.Hours.Should().Be("*/2");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("0 0 12")]
    [InlineData("0 0 12 * *")]
    [InlineData("0 0 12 * * ? * * extra")]
    public void Parse_WithInvalidFormat_ShouldThrowArgumentException(string expression)
    {
        // Act & Assert
        FluentActions.Invoking(() => CronParser.Parse(expression))
            .Should().Throw<ArgumentException>()
            .WithParameterName("cronExpression")
            .WithMessage("Invalid Cron expression format*");
    }

    [Theory]
    [InlineData("0 0 0 * * ? *", "每天")]
    [InlineData("0 0 8 * * ? *", "每天上午8点")]
    [InlineData("0 0 0 ? * 1 *", "每周一")]
    [InlineData("0 0 15 ? * 1,3 *", "每周一和周三下午3点")]
    [InlineData("0 0 0 15 * ? *", "每月15号")]
    [InlineData("0 */5 * * * ? *", "每5分钟")]
    [InlineData("0 30 */2 * * ? *", "每2小时在30分")]
    [InlineData("0 0 0 */3 * ? *", "每3天")]
    [InlineData("0 0 0 ? * 1,3,5 *", "每周一、三、五")]
    [InlineData("0 30 10 5,15 * ? *", "每月5号和15号上午10点30分")]
    public void ToChineseDescription_WithValidExpression_ShouldReturnCorrectDescription(string expression, string expected)
    {
        // Act
        var result = CronParser.ToChineseDescription(expression);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ToChineseDescription_WithInvalidExpression_ShouldThrowArgumentException()
    {
        // Arrange
        var expression = "invalid";

        // Act & Assert
        FluentActions.Invoking(() => CronParser.ToChineseDescription(expression))
            .Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("0 0 12 * * ? *", true)]
    [InlineData("30 15 10 ? * 1-5 2023", true)]
    [InlineData("0 */5 * * * ? *", true)]
    [InlineData("0 0,15,30,45 9,12,15,18 5,15,25 3,6,9,12 ? *", true)]
    [InlineData("invalid", false)]
    [InlineData("0 0 25 * * ? *", false)] // Invalid hour
    [InlineData("0 60 12 * * ? *", false)] // Invalid minute
    [InlineData("60 0 12 * * ? *", false)] // Invalid second
    [InlineData("0 0 12 32 * ? *", false)] // Invalid day of month
    [InlineData("0 0 12 * 13 ? *", false)] // Invalid month
    [InlineData("0 0 12 ? * 8 *", false)] // Invalid day of week
    [InlineData("0 0 12 * * ? 1969", false)] // Invalid year
    public void IsValid_WithVariousExpressions_ShouldReturnExpectedResult(string expression, bool expected)
    {
        // Act
        var result = CronParser.IsValid(expression);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void GetNextExecutionTime_WithValidExpression_ShouldReturnFutureTime()
    {
        // Arrange
        var expression = "0 0 12 * * ? *"; // Daily at noon
        var fromTime = new DateTime(2023, 1, 1, 10, 0, 0);

        // Act
        var result = CronParser.GetNextExecutionTime(expression, fromTime);

        // Assert
        result.Should().BeAfter(fromTime);
    }

    [Fact]
    public void GetNextExecutionTime_WithInvalidExpression_ShouldThrowArgumentException()
    {
        // Arrange
        var expression = "invalid";

        // Act & Assert
        FluentActions.Invoking(() => CronParser.GetNextExecutionTime(expression))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetNextExecutionTime_WithoutFromTime_ShouldUseCurrentTime()
    {
        // Arrange
        var expression = "0 0 12 * * ? *";
        var beforeCall = DateTime.Now;

        // Act
        var result = CronParser.GetNextExecutionTime(expression);

        // Assert
        result.Should().BeAfter(beforeCall);
    }
}

public class CronExpressionTests
{
    [Fact]
    public void Constructor_ShouldSetDefaultValues()
    {
        // Act
        var expression = new CronExpression();

        // Assert
        expression.Seconds.Should().Be("0");
        expression.Minutes.Should().Be("0");
        expression.Hours.Should().Be("0");
        expression.DayOfMonth.Should().Be("*");
        expression.Month.Should().Be("*");
        expression.DayOfWeek.Should().Be("?");
        expression.Year.Should().Be("*");
    }

    [Fact]
    public void ToString_ShouldReturnFormattedExpression()
    {
        // Arrange
        var expression = new CronExpression
        {
            Seconds = "30",
            Minutes = "15",
            Hours = "10",
            DayOfMonth = "?",
            Month = "*",
            DayOfWeek = "1-5",
            Year = "2023"
        };

        // Act
        var result = expression.ToString();

        // Assert
        result.Should().Be("30 15 10 ? * 1-5 2023");
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var expression = new CronExpression();

        // Act
        expression.Seconds = "30";
        expression.Minutes = "15";
        expression.Hours = "10";
        expression.DayOfMonth = "?";
        expression.Month = "6";
        expression.DayOfWeek = "1-5";
        expression.Year = "2023";

        // Assert
        expression.Seconds.Should().Be("30");
        expression.Minutes.Should().Be("15");
        expression.Hours.Should().Be("10");
        expression.DayOfMonth.Should().Be("?");
        expression.Month.Should().Be("6");
        expression.DayOfWeek.Should().Be("1-5");
        expression.Year.Should().Be("2023");
    }
}