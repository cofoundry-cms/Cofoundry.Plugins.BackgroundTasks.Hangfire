using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cofoundry.Core.BackgroundTasks;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    public class HangfireBackgroundTaskScheduler : IBackgroundTaskScheduler
    {
        #region public methods

        public IBackgroundTaskScheduler RegisterRecurringTask<TTask>(int days, int atHour, int atMinute) where TTask : IRecurringBackgroundTask
        {
            string cronExpression = string.Format("{2} {1} */{0} * *", days, atHour, atMinute);
            RegisterRecurringTask<TTask>(cronExpression);

            return this;
        }

        public IBackgroundTaskScheduler RegisterRecurringTask<TTask>(int hours = 0, int minute = 0) where TTask : IRecurringBackgroundTask
        {
            string cronExpression = string.Format("{1} */{0} * * *", hours, minute);
            RegisterRecurringTask<TTask>(cronExpression);

            return this;

        }

        public IBackgroundTaskScheduler RegisterRecurringTask<TTask>(int minutes) where TTask : IRecurringBackgroundTask
        {
            string cronExpression = string.Format("*/{0} * * * *", minutes);
            RegisterRecurringTask<TTask>(cronExpression);

            return this;
        }

        public IBackgroundTaskScheduler DeregisterRecurringTask<TTask>() where TTask : IRecurringBackgroundTask
        {
            RecurringJob.RemoveIfExists(GetJobId<TTask>());

            return this;
        }

        #endregion

        #region private methods

        private string GetJobId<TTask>()
        {
            return typeof(TTask).FullName;
        }

        private void RegisterRecurringTask<TTask>(string cronExpression) where TTask : IRecurringBackgroundTask
        {
            RecurringJob.AddOrUpdate<InjectableTaskWrapper<TTask>>(GetJobId<TTask>(), t => t.Execute(), cronExpression);
        }

        #endregion
    }
}
