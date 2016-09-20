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
    /// Exposes methods that can be used to configure hangfire and bootstrap
    /// self-registering background tasks.
    /// </summary>
    public interface IHangfireBackgroundTaskInitializer
    {
        #region public methods

        void Initialize();

        #endregion
    }
}
