using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Dashboard;
using Hangfire.Annotations;
using Cofoundry.Domain;
using Cofoundry.Web;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var service = IckyDependencyResolution.ResolveFromMvcContext<IUserContextService>();
            var userContext = service.GetCurrentContext();
            return userContext.IsCofoundryUser();
        }
    }
}
