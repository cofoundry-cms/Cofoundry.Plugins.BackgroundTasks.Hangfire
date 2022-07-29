using Cofoundry.Web;
using Microsoft.AspNetCore.Builder;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire;

/// <summary>
/// The main auto-startup task for initializing hangfire.
/// </summary>
/// <remarks>
/// To customize the startup process you can override IHangfireBackgroundTaskInitializer
/// and IHangfireServerInitializer implementations, or just create your own plugin.
/// </remarks>
public class HangfireStartupConfigurationTask
    : IRunBeforeStartupConfigurationTask
    , IRunAfterStartupConfigurationTask
{
    private readonly IHangfireServerInitializer _hangfireServerInitializer;

    public HangfireStartupConfigurationTask(
        IHangfireServerInitializer hangfireServerInitializer
        )
    {
        _hangfireServerInitializer = hangfireServerInitializer;
    }

    public int Ordering { get; } = (int)StartupTaskOrdering.Normal;

    public ICollection<Type> RunBefore { get; } = new Type[] { typeof(AddEndpointRoutesStartupConfigurationTask) };

    public ICollection<Type> RunAfter { get; } = new Type[] { typeof(AutoUpdateMiddlewareStartupConfigurationTask) };

    public void Configure(IApplicationBuilder app)
    {
        _hangfireServerInitializer.Initialize(app);
        app.UseMiddleware<HangfireInitializationMiddleware>();
    }
}