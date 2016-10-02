using Cofoundry.Core.DependencyInjection;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    /// <summary>
    /// Custom job activator to make Hangfire support DI.
    /// </summary>
    /// <remarks>
    /// see http://docs.hangfire.io/en/latest/background-methods/using-ioc-containers.html
    /// </remarks>
    public class ContainerJobActivator : JobActivator
    {
        private readonly IResolutionContext _resolutionContext;

        public ContainerJobActivator(
            IResolutionContext resolutionContext
            )
        {
            _resolutionContext = resolutionContext;
        }

        public override object ActivateJob(Type type)
        {
            object job = null;

            // Bit of a hack here to wrap injectable tasks because
            // hangire doesn't support child contexts.
            if (type.IsGenericType 
                && (type.GetGenericTypeDefinition() == typeof(InjectableTaskWrapper<>)
                 || type.GetGenericTypeDefinition() == typeof(InjectableAsyncTaskWrapper<>))
                )
            {
                job = Activator.CreateInstance(type, _resolutionContext);
            }
            else if (_resolutionContext.IsRegistered(type))
            {
                job = _resolutionContext.Resolve(type);
            }
            else
            {
                job = Activator.CreateInstance(type);
            }

            return job;
        }
    }
}
