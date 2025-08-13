# CronTools

A fluent API for building Cron expressions in .NET.

一个用于构建Cron表达式的流畅API库，基于.NET 8开发。

## 特性

- 🚀 **流畅的API设计** - 使用方法链式调用构建Cron表达式
- ✅ **类型安全** - 编译时验证参数范围
- 🔍 **表达式解析** - 解析和验证现有的Cron表达式
- 📅 **预设模板** - 提供常用的Cron表达式预设
- 🧪 **全面测试** - 包含完整的单元测试覆盖

## 快速开始
### 基本用法

```csharp
using CronTools;

// 每天上午8:30执行
var dailyExpression = CronTools.Create()
    .AtHour(8)
    .AtMinute(30)
    .Daily()
    .Build();
// 结果: "0 30 8 * * ? *"

// 每周一上午9:00执行
var mondayExpression = CronTools.Create()
    .OnMonday()
    .AtHour(9)
    .AtMinute(0)
    .Build();
// 结果: "0 0 9 ? * 1 *"

// 每月15号下午2:30执行
var monthlyExpression = CronTools.Create()
    .OnDay(15)
    .AtHour(14)
    .AtMinute(30)
    .Build();
// 结果: "0 30 14 15 * ? *"
```

### 高级用法

```csharp
// 工作日上午9点和下午5点执行var businessHours = CronTools.Create()
    .OnWeekdays()
    .AtHours(9, 17)
    .AtMinute(0)
    .Build();
// 结果: "0 0 9,17 ? * 1,2,3,4,5 *"

// 复杂的调度表达式
var complexSchedule = CronTools.Create()
    .AtSecond(30)
    .AtMinutes(0, 15, 30, 45)
    .AtHours(9, 12, 15, 18)
    .OnDays(5, 15, 25)
    .InMonths(3, 6, 9, 12)
    .Build();
// 结果: "30 0,15,30,45 9,12,15,18 5,15,25 3,6,9,12 ? *"

// 范围表达式var rangeExample = CronTools.Create()
    .OnDaysBetween(10, 20)
    .InMonthsBetween(4, 10)
    .AtHour(14)
    .AtMinute(0)
    .Build();
// 结果: "0 0 14 10-20 4-10 ? *"
```

### 扩展方法

```csharp
// 使用便利的扩展方法var dailyAtNoon = CronTools.Create().AtNoon().Build();
var dailyAtMidnight = CronTools.Create().AtMidnight().Build();
var weeklyMeeting = CronTools.Create().WeeklyAt(1, 10, 0).Build(); // 周一10点
var monthlyReport = CronTools.Create().MonthlyAt(1, 9, 0).Build(); // 每月1号9点```

### 预设表达式
```csharp
// 使用预定义的常用表达式Console.WriteLine(CronPresets.EveryMinute);     // "0 * * * * ? *"
Console.WriteLine(CronPresets.Every5Minutes);   // "0 */5 * * * ? *"
Console.WriteLine(CronPresets.EveryHour);       // "0 0 * * * ? *"
Console.WriteLine(CronPresets.Daily);           // "0 0 0 * * ? *"
Console.WriteLine(CronPresets.WeekdaysAt9AM);   // "0 0 9 ? * 1,2,3,4,5 *"
```

### 表达式解析和验证

```csharp
// 解析现有的Cron表达式var expression = "0 30 9 * * ? *";
var parsed = CronParser.Parse(expression);
Console.WriteLine($"小时: {parsed.Hours}");     // "9"
Console.WriteLine($"分钟: {parsed.Minutes}");   // "30"

// 验证表达式var isValid = CronParser.IsValid("0 30 9 * * ? *");
Console.WriteLine($"表达式有效: {isValid}");     // True

// 获取下次执行时间
var nextExecution = CronParser.GetNextExecutionTime("0 30 9 * * ? *");
Console.WriteLine($"下次执行: {nextExecution}");

// 使用扩展方法验证构建器var builder = CronTools.Create().DailyAt(9, 30);
var builderIsValid = builder.IsValid();
var nextTime = builder.GetNextExecutionTime();
```

## API 参考
### CronTools 方法

#### 时间设置
- `AtSecond(int second)` - 设置秒（0-59）
- `AtSeconds(params int[] seconds)` - 设置多个秒数
- `AtMinute(int minute)` - 设置分钟（0-59）
- `AtMinutes(params int[] minutes)` - 设置多个分钟
- `AtHour(int hour)` - 设置小时（0-23）
- `AtHours(params int[] hours)` - 设置多个小时
#### 日期设置
- `OnDay(int day)` - 设置日期（1-31）
- `OnDays(params int[] days)` - 设置多个日期
- `OnDaysBetween(int start, int end)` - 设置日期范围
- `InMonth(int month)` - 设置月份（1-12）
- `InMonths(params int[] months)` - 设置多个月份
- `InMonthsBetween(int start, int end)` - 设置月份范围
- `InYear(int year)` - 设置年份（1970-2099）
- `InYears(params int[] years)` - 设置多个年份
- `InYearsBetween(int start, int end)` - 设置年份范围

#### 星期设置
- `OnWeekday(int day)` - 设置星期（1-7，1=周一）
- `OnWeekdays(params int[] days)` - 设置多个星期
- `OnMonday()` - 设置周一
- `OnTuesday()` - 设置周二
- `OnWednesday()` - 设置周三
- `OnThursday()` - 设置周四
- `OnFriday()` - 设置周五
- `OnSaturday()` - 设置周六
- `OnSunday()` - 设置周日
- `OnWeekdays()` - 设置工作日（周一至周五）
- `OnWeekends()` - 设置周末（周六和周日）
#### 便利方法
- `Daily()` - 每天执行
- `Hourly()` - 每小时执行
- `Hourly(int atMinute)` - 每小时在指定分钟执行
- `EveryMinute()` - 每分钟执行
- `EveryNMinutes(int interval)` - 每N分钟执行
- `EveryNHours(int interval)` - 每N小时执行
- `EveryNDays(int interval)` - 每N天执行
#### 构建
- `Build()` - 构建Cron表达式字符串

### 扩展方法

- `DailyAt(int hour, int minute, int second = 0)` - 每天在指定时间执行
- `WeeklyAt(int dayOfWeek, int hour, int minute, int second = 0)` - 每周在指定时间执行
- `MonthlyAt(int dayOfMonth, int hour, int minute, int second = 0)` - 每月在指定时间执行
- `AtMidnight()` - 每天午夜执行
- `AtNoon()` - 每天中午执行
- `OnFirstDayOfMonth(int hour = 0, int minute = 0)` - 每月第一天执行
- `OnLastDayOfMonth(int hour = 0, int minute = 0)` - 每月最后一天执行
- `IsValid()` - 验证构建的表达式
- `GetNextExecutionTime(DateTime? fromTime = null)` - 获取下次执行时间

### CronParser 方法

- `Parse(string cronExpression)` - 解析Cron表达式
- `IsValid(string cronExpression)` - 验证Cron表达式
- `GetNextExecutionTime(string cronExpression, DateTime? fromTime = null)` - 获取下次执行时间

## Cron表达式格式
本库使用7字段的Cron表达式格式：

```
秒 分 时 日 月 周 年
```

- **秒**: 0-59
- **分**: 0-59
- **时**: 0-23
- **日**: 1-31
- **月**: 1-12
- **周**: 1-7 (1=周一, 7=周日)
- **年**: 1970-2099

### 特殊字符

- `*` - 匹配所有值
- `?` - 不指定值（用于日和周字段的互斥）
- `,` - 分隔多个值（如：1,3,5）
- `-` - 指定范围（如：1-5）
- `/` - 指定间隔（如：0/5表示每5个单位）

## 贡献

欢迎贡献代码！请遵循以下步骤：
1. Fork 项目
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 创建 Pull Request

### 开发指南
- 遵循现有的代码风格
- 为新功能添加单元测试
- 更新文档和示例
- 确保所有测试通过
