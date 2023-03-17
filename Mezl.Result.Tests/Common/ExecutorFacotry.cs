using Mezl.Result.Handler;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mezl.Result.Tests.Common
{
    internal static class ExecutorFactory
    {
        internal static IRequestExecutor CreateExecutor()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHandlers();

            var executor = serviceCollection.BuildServiceProvider().GetService<IRequestExecutor>();
            return executor;
        }
    }
}
