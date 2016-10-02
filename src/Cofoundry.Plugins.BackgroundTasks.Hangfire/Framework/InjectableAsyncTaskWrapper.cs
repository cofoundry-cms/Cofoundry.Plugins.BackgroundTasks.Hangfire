
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cofoundry.Core.BackgroundTasks;
using Cofoundry.Core.DependencyInjection;
using Cofoundry.Core.ErrorLogging;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    /// <summary>
    /// Used to wrap task execution in a dispoable DI child context since
    /// hangfire does not yet support child contexts.
    /// </summary>
    /// <typeparam name="TTask">Type fo task to execute</typeparam>
    public class InjectableAsyncTaskWrapper<TTask> where TTask : IAsyncBackgroundTask
    {
        private readonly IResolutionContext _resolutionContext;

        public InjectableAsyncTaskWrapper(
            IResolutionContext resolutionContext
            )
        {
            _resolutionContext = resolutionContext;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task ExecuteAsync()
        {
            using (var childContext = _resolutionContext.CreateChildContext())
            {
                try
                {
                    var task = childContext.Resolve<TTask>();
                    await task.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    // I couldn't get hangfire error logging to work so I'm just catching any exceptions here
                    // and sending them to the error logger.
                    var errorLoggingService = childContext.Resolve<IErrorLoggingService>();
                    errorLoggingService.Log(ex);

                    throw;
                }
            }
        }
    }
}
