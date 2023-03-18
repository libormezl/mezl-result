using Mezl.Result.Handler;
using Mezl.Result.Reasons;
using Mezl.Result.Tests.Common;

namespace Mezl.Result.Tests;

public class AsyncRequestWithoutResponseHandlerTest
{
    private record ExampleRequest(bool IsOk = true) : IRequest { }

    [Lifecycle(Lifecycle.Singleton)]
    private class ExampleRequestHandler : IAsyncRequestHandler<ExampleRequest>
    {
        public Task<R> HandleAsync(ExampleRequest request, CancellationToken cancellationToken)
        {
            var result = request.IsOk ? R.Success : Reason.New<ReasonNotFound>();
            return Task.FromResult(result);
        }
    }

    [Lifecycle(Lifecycle.Singleton)]
    private class ExampleRequestValidator : IValidator<ExampleRequest>
    {
        public R Validate(ExampleRequest request)
        {
            return R.Success;
        }
    }

    [Fact]
    public async Task AsyncRequestWithoutResponseHandler_Success()
    {
        var result = await ExecutorFactory.CreateExecutor().ExecuteAsync(new ExampleRequest(), CancellationToken.None);

        Assert.Equal(R.Success, result);
    }

    [Fact]
    public async Task AsyncRequestWithoutResponseHandler__FailedWithReason()
    {
        var result = await ExecutorFactory.CreateExecutor().ExecuteAsync(new ExampleRequest(false), CancellationToken.None);

        Assert.True(result.Is<ReasonNotFound>());
    }
}