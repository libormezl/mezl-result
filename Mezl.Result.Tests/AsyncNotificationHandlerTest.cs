using Mezl.Result.Handler;
using Mezl.Result.Tests.Common;

namespace Mezl.Result.Tests;

public class AsyncNotificationHandlerTest
{
    private record ExampleNotification : INotification { }

    [Lifecycle(Lifecycle.Singleton)]
    private class ExampleRequestHandler : IAsyncNotificationHandler<ExampleNotification>
    {
        public Task HandleAsync(ExampleNotification request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task AsyncRequestWithoutResponseHandler_Success()
    {
        ExecutorFactory.CreateExecutor().Notify(new ExampleNotification());
    }
}