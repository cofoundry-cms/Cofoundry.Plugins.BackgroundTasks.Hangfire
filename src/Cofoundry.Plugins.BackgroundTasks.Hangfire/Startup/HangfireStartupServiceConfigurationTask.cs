using Cofoundry.Web;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Cofoundry.Core;
using Hangfire;
using Hangfire.SqlServer;
using Cofoundry.Core.AutoUpdate;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    /// <summary>
    /// Configuration builder step to add hangfire services and basic 
    /// sql server configuration.
    /// </summary>
    public class HangfireStartupServiceConfigurationTask : IStartupServiceConfigurationTask
    {
        private readonly DatabaseSettings _databaseSettings;
        private readonly IAutoUpdateService _autoUpdateService;

        public HangfireStartupServiceConfigurationTask(
            DatabaseSettings databaseSettings,
            IAutoUpdateService autoUpdateService
            )
        {
            _databaseSettings = databaseSettings;
            _autoUpdateService = autoUpdateService;
        }

        public void ConfigureServices(IMvcBuilder mvcBuilder)
        {
            // We have to block here as service configuration is not async.
            var isDbLocked = _autoUpdateService.IsLockedAsync().GetAwaiter().GetResult();
            var connectionString = _databaseSettings.ConnectionString;

            mvcBuilder
                .Services
                .AddHangfire(configuration => configuration
                    .UseSqlServerStorage(connectionString, new SqlServerStorageOptions()
                    {
                        PrepareSchemaIfNecessary = isDbLocked
                    })
                    .UseFilter(new AutomaticRetryAttribute { Attempts = 0 })
                    );
        }
    }
}
