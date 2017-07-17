using Cofoundry.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    /// <summary>
    /// Settings for HangFire configuration
    /// </summary>
    public class HangfireSettings : PluginConfigurationSettingsBase
    {
        /// <summary>
        /// Prevents the HangFire server being configuted and started. Defaults
        /// to false.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Enables the HangFire dashboard for Cofoundry admin users 
        /// at /admin/hangfire. Defaults to false.
        /// </summary>
        public bool EnableHangfireDashboard { get; set; }
    }
}
