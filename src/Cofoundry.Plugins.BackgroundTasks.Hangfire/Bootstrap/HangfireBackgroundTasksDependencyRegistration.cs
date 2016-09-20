using Cofoundry.Core.BackgroundTasks;
using Cofoundry.Core.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    public class HangfireBackgroundTasksDependencyRegistration : IDependencyRegistration
    {
        public void Register(IContainerRegister container)
        {
            container
                .RegisterType<IHangfireBackgroundTaskInitializer, HangfireBackgroundTaskInitializer>()
                .RegisterType<IHangfireServerInitializer, HangfireServerInitializer>()
                .RegisterType<IBackgroundTaskScheduler, HangfireBackgroundTaskScheduler>()
                ;
        }
    }
}
