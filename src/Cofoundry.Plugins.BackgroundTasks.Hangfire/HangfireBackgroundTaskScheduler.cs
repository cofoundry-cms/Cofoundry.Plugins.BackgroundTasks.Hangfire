using Cofoundry.Core.BackgroundTasks;
using Hangfire;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire;

public class HangfireBackgroundTaskScheduler : IBackgroundTaskScheduler
{
    public IBackgroundTaskScheduler RegisterRecurringTask<TTask>(int days, int atHour, int atMinute) where TTask : IRecurringBackgroundTask
    {
        ValidateDailyTaskParameters(days, atHour, atMinute);

        string cronExpression = string.Format("{2} {1} */{0} * *", days, atHour, atMinute);
        RegisterRecurringTask<TTask>(cronExpression);

        return this;
    }

    public IBackgroundTaskScheduler RegisterRecurringTask<TTask>(int hours = 0, int minute = 0) where TTask : IRecurringBackgroundTask
    {
        ValidateHourlyTaskParameters(hours, minute);

        if (hours == 0 && minute > 0)
        {
            return RegisterRecurringTask<TTask>(minute);
        }

        string cronExpression = string.Format("{1} */{0} * * *", hours, minute);
        RegisterRecurringTask<TTask>(cronExpression);

        return this;
    }

    public IBackgroundTaskScheduler RegisterRecurringTask<TTask>(int minutes) where TTask : IRecurringBackgroundTask
    {
        ValidateMinuteTaskParameters(minutes);

        if (minutes > 59)
        {
            var hours = minutes / 60;
            minutes = minutes % 60;
            return RegisterRecurringTask<TTask>(hours, minutes);
        }

        string cronExpression = string.Format("*/{0} * * * *", minutes);
        RegisterRecurringTask<TTask>(cronExpression);

        return this;
    }

    public IBackgroundTaskScheduler DeregisterRecurringTask<TTask>() where TTask : IRecurringBackgroundTask
    {
        RecurringJob.RemoveIfExists(GetJobId<TTask>());

        return this;
    }

    public IBackgroundTaskScheduler RegisterAsyncRecurringTask<TTask>(int days, int atHour = 0, int atMinute = 0) where TTask : IAsyncRecurringBackgroundTask
    {
        ValidateDailyTaskParameters(days, atHour, atMinute);

        string cronExpression = string.Format("{2} {1} */{0} * *", days, atHour, atMinute);
        RegisterAsyncRecurringTask<TTask>(cronExpression);

        return this;
    }

    public IBackgroundTaskScheduler RegisterAsyncRecurringTask<TTask>(int hours = 0, int minute = 0) where TTask : IAsyncRecurringBackgroundTask
    {
        ValidateHourlyTaskParameters(hours, minute);

        if (hours == 0 && minute > 0)
        {
            return RegisterAsyncRecurringTask<TTask>(minute);
        }

        string cronExpression = string.Format("{1} */{0} * * *", hours, minute);
        RegisterAsyncRecurringTask<TTask>(cronExpression);

        return this;
    }

    public IBackgroundTaskScheduler RegisterAsyncRecurringTask<TTask>(int minutes) where TTask : IAsyncRecurringBackgroundTask
    {
        ValidateMinuteTaskParameters(minutes);

        if (minutes > 59)
        {
            var hours = minutes / 60;
            minutes = minutes % 60;
            return RegisterAsyncRecurringTask<TTask>(hours, minutes);
        }

        string cronExpression = string.Format("*/{0} * * * *", minutes);
        RegisterAsyncRecurringTask<TTask>(cronExpression);

        return this;
    }

    public IBackgroundTaskScheduler DeregisterAsyncRecurringTask<TTask>() where TTask : IAsyncRecurringBackgroundTask
    {
        RecurringJob.RemoveIfExists(GetJobId<TTask>());

        return this;
    }

    private string GetJobId<TTask>()
    {
        return typeof(TTask).FullName;
    }

    private void RegisterRecurringTask<TTask>(string cronExpression) where TTask : IRecurringBackgroundTask
    {
        RecurringJob.AddOrUpdate<TTask>(GetJobId<TTask>(), t => t.Execute(), cronExpression);
    }

    private void RegisterAsyncRecurringTask<TTask>(string cronExpression) where TTask : IAsyncRecurringBackgroundTask
    {
        RecurringJob.AddOrUpdate<TTask>(GetJobId<TTask>(), t => t.ExecuteAsync(), cronExpression);
    }

    private void ValidateDailyTaskParameters(int days, int atHour, int atMinute)
    {
        ValidateNotNegative(days, nameof(days));
        ValidateNotNegative(atHour, nameof(atHour));
        ValidateNotNegative(atMinute, nameof(atMinute));

        if (days == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(days), "Daily recurring tasks need to have an interval of at least 1 day.");
        }
    }

    private void ValidateHourlyTaskParameters(int hours, int minute)
    {
        ValidateNotNegative(hours, nameof(hours));
        ValidateNotNegative(minute, nameof(minute));

        if (hours == 0 && minute == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hours), "Recurring tasks need to have an interval of at least 1 minute.");
        }

        if (hours > 23)
        {
            throw new ArgumentOutOfRangeException(nameof(hours), "Recurring tasks with an interval of 24 hours or more should be scheduled using a daily interval instead.");
        }

        if (minute > 59)
        {
            throw new ArgumentOutOfRangeException(nameof(hours), "Recurring tasks with an interval measured in hours and minutes cannot have an minute component greater than 59 minutes.");
        }
    }

    private void ValidateMinuteTaskParameters(int minutes)
    {
        ValidateNotNegative(minutes, nameof(minutes));
        if (minutes == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minutes), "Recurring tasks need to have an interval of at least 1 minute.");
        }
    }

    private void ValidateNotNegative(int number, string argumentName)
    {
        if (number < 0)
        {
            throw new ArgumentOutOfRangeException(argumentName, "Recurring tasks cannot set the interval to negative numbers.");
        }
    }
}
