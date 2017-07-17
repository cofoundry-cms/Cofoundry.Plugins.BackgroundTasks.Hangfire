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

        public HangfireServerInitializer(
            HangfireSettings hangfireSettings
            )
        {
            _hangfireSettings = hangfireSettings;
        }

        public void Initialize(IApplicationBuilder app)
        {
            // Allow hangfire to be disabled, e.g. when connecting from dev to a production db.
            if (_hangfireSettings.Disabled) return;

            app.UseHangfireServer();

            if (_hangfireSettings.EnableHangfireDashboard)
            {
                app.UseHangfireDashboard("/admin/hangfire", new DashboardOptions
                {
                    Authorization = new IDashboardAuthorizationFilter[] { new HangfireDashboardAuthorizationFilter() },
                    AppPath = "/admin"
                });
            }
        }
    }
}
