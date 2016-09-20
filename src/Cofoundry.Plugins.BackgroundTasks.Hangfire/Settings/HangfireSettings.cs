using Cofoundry.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    public class HangfireSettings : PluginConfigurationSettingsBase
    {
        public bool DisableHangfire { get; set; }
        public bool EnableHangfireDashboard { get; set; }
    }
}
