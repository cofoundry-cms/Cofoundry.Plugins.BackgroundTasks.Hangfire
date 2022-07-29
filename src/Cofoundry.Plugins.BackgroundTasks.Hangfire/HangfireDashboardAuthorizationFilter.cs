using Cofoundry.Domain;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire;

public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        var service = (IUserContextService)context.GetHttpContext().RequestServices.GetService(typeof(IUserContextService));

        // Hangfire does not support async auth:
        // https://github.com/HangfireIO/Hangfire/issues/827
        var userContext = service
            .GetCurrentContextByUserAreaAsync(CofoundryAdminUserArea.AreaCode)
            .ConfigureAwait(false).GetAwaiter().GetResult();

        return userContext.IsCofoundryUser();
    }
}
