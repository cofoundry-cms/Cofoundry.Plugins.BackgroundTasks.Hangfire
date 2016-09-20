using Cofoundry.Core;
using Cofoundry.Core.AutoUpdate;
using Cofoundry.Core.DependencyInjection;
using Cofoundry.Domain.Data;
using Cofoundry.Web;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Owin;
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
        private readonly IAutoUpdateService _autoUpdateService;

        public HangfireServerInitializer(
            HangfireSettings hangfireSettings,
            IAutoUpdateService autoUpdateService
            )
        {
            _hangfireSettings = hangfireSettings;
            _autoUpdateService = autoUpdateService;
        }

        public void Initialize(IAppBuilder app)
        {
            // Allow hangfire to be disabled, e.g. when connecting from dev to a production db.
            if (_hangfireSettings.DisableHangfire) return;

            // Register background tasks using the root context, since the task scheduler
            // needs the root context to create its own child contexts.
            var resolutionContext = IckyDependencyResolution.ResolveFromRootContext<IResolutionContext>();
            JobActivator.Current = new ContainerJobActivator(resolutionContext);

            var isDbLocked = _autoUpdateService.IsLocked();

            GlobalConfiguration.Configuration.UseSqlServerStorage(DbConstants.ConnectionStringName, new SqlServerStorageOptions()
            {
                PrepareSchemaIfNecessary = !isDbLocked
            });

            if (_hangfireSettings.EnableHangfireDashboard)
            {
                app.UseHangfireDashboard("/admin/hangfire", new DashboardOptions
                {
                    Authorization = new IDashboardAuthorizationFilter[] { new HangfireDashboardAuthorizationFilter() },
                    AppPath = "/admin"
                });
            }

            app.UseHangfireServer();
        }
    }
}
