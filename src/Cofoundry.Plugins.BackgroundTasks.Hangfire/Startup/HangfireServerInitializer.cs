using Cofoundry.Core;
using Cofoundry.Domain;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    /// <summary>
    /// Used to initialize the hangfire server. Override this to fully
    /// customize the server initialization process.
    /// </summary>
    public class HangfireServerInitializer : IHangfireServerInitializer
    {
        private readonly HangfireSettings _hangfireSettings;
        private readonly AdminSettings _adminSettings;

        public HangfireServerInitializer(
            HangfireSettings hangfireSettings,
            AdminSettings adminSettings
            )
        {
            _hangfireSettings = hangfireSettings;
            _adminSettings = adminSettings;
        }

        public void Initialize(IApplicationBuilder app)
        {
            // Allow hangfire to be disabled, e.g. when connecting from dev to a production db.
            if (_hangfireSettings.Disabled) return;

            app.UseHangfireServer();

            if (_hangfireSettings.EnableHangfireDashboard && !_adminSettings.Disabled)
            {
                var adminPath = RelativePathHelper.Combine(_adminSettings.DirectoryName, "hangfire");
                app.UseHangfireDashboard(adminPath, new DashboardOptions
                {
                    Authorization = new IDashboardAuthorizationFilter[] { new HangfireDashboardAuthorizationFilter() },
                    AppPath = "/" + _adminSettings.DirectoryName
                });
            }
        }
    }
}
