using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    /// <summary>
    /// Exposes methods that can be used to configure hangfire and bootstrap
    /// self-registering background tasks.
    /// </summary>
    public interface IHangfireBackgroundTaskInitializer
    {
        void Initialize();
    }
}
