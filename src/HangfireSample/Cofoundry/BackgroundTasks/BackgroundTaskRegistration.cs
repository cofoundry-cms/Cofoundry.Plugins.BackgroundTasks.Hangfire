using Cofoundry.Core.BackgroundTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireSample.Cofoundry.BackgroundTasks
{
    public class BackgroundTaskRegistration : IBackgroundTaskRegistration
    {
        public void Register(IBackgroundTaskScheduler scheduler)
        {
            scheduler.RegisterAsyncRecurringTask<ProductAddingBackgroundTask>(1);
        }
    }
}
