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
    public interface IHangfireServerInitializer
    {
        void Initialize(IAppBuilder app);
    }
}
