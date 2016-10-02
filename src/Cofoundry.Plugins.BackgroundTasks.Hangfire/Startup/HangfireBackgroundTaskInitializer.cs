    using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Logging;
using Cofoundry.Core.BackgroundTasks;
using Cofoundry.Core.DependencyInjection;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    /// <summary>
    /// Used to bootstrap self-registering background tasks.
    /// </summary>
    public class HangfireBackgroundTaskInitializer : IHangfireBackgroundTaskInitializer
    {
        #region constructor

        private readonly HangfireSettings _hangfireSettings;
        private readonly IBackgroundTaskRegistration[] _backgroundTaskRegistrations;
        private readonly IBackgroundTaskScheduler _backgroundTaskScheduler;

        public HangfireBackgroundTaskInitializer(
            HangfireSettings hangfireSettings,
            IBackgroundTaskRegistration[] backgroundTaskRegistrations,
            IBackgroundTaskScheduler backgroundTaskScheduler
            )
        {
            _hangfireSettings = hangfireSettings;
            _backgroundTaskRegistrations = backgroundTaskRegistrations;
            _backgroundTaskScheduler = backgroundTaskScheduler;
        }

        #endregion

        #region public methods

        public void Initialize()
        {
            if (_hangfireSettings.DisableHangfire) return;

            foreach (var registration in _backgroundTaskRegistrations)
            {
                registration.Register(_backgroundTaskScheduler);
            }
        }

        #endregion
    }
}
