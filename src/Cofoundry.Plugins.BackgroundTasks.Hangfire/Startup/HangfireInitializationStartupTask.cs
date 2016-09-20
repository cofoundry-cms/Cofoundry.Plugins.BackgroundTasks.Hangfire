using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cofoundry.Core.AutoMapper;
using Cofoundry.Web;
using Cofoundry.Core.AutoUpdate;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Cofoundry.Domain.Data;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    /// <summary>
    /// The main auto-startup task for initializing hangfire.
    /// </summary>
    /// <remarks>
    /// To customize the startup process you can override IHangfireBackgroundTaskInitializer
    /// and IHangfireServerInitializer implementations, or just create your own plugin.
    /// </remarks>
    public class HangfireInitializationStartupTask : IStartupTask
    {
        private readonly IHangfireBackgroundTaskInitializer _hangfireBackgroundTaskInitializer;
        private readonly IHangfireServerInitializer _hangfireServerInitializer;
        
        public HangfireInitializationStartupTask(
            IHangfireBackgroundTaskInitializer hangfireBackgroundTaskInitializer,
            IHangfireServerInitializer hangfireServerInitializer
            )
        {
            _hangfireBackgroundTaskInitializer = hangfireBackgroundTaskInitializer;
            _hangfireServerInitializer = hangfireServerInitializer;
        }

        public int Ordering
        {
            get { return (int)StartupTaskOrdering.Normal; }
        }

        public void Run(IAppBuilder app)
        {
            _hangfireServerInitializer.Initialize(app);
            _hangfireBackgroundTaskInitializer.Initialize();
        }
    }
}