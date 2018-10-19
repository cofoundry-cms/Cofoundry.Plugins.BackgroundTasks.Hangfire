using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Cofoundry.Plugins.BackgroundTasks.Hangfire
{
    public class HangfireInitializationMiddleware
    {
        private static bool isInitialized = false;
        private static object _isInitializedLock = new object();
        private readonly RequestDelegate _next;

        public HangfireInitializationMiddleware(
            RequestDelegate next
            )
        {
            _next = next;
        }

        public async Task Invoke(HttpContext cx)
        {
            bool runInitialize = false;

            if (!isInitialized)
            {
                lock (_isInitializedLock)
                {
                    if (!isInitialized)
                    {
                        isInitialized = true;
                        runInitialize = true;
                    }
                }

                if (runInitialize)
                {
                    try
                    {
                        var initializer = cx.RequestServices.GetService<IHangfireBackgroundTaskInitializer>();
                        initializer.Initialize();
                    }
                    catch (Exception ex)
                    {
                        isInitialized = false;
                        throw;
                    }
                }
            }

            await _next.Invoke(cx);
        }
    }
}
