using Mezl.Result.Extensions;
using Mezl.Result.Handler;
using Mezl.Result.Reasons;
using Mezl.Result.Tests.Common;

namespace Mezl.Result.Tests;

public class AsyncRequestWithResponseHandlerTest
{
    private record ExampleRequest(bool IsOk = true, bool ValidationFailed = false) : IRequest<string> { }

    [Lifecycle(Lifecycle.Singleton)]
    private class ExampleRequestHandler : IAsyncRequestHandler<ExampleRequest, string>
    {
        public Task<R<string>> HandleAsync(ExampleRequest request, CancellationToken cancellationToken)
        {
            var result = request.IsOk ? (R<string>)"aaaa" : Reason.New<ReasonNotFound>();
            return Task.FromResult(result);
        }
    }

    [Lifecycle(Lifecycle.Singleton)]
    private class ExampleRequestValidator : IAsyncValidator<ExampleRequest>
    {
        public Task<R> ValidateAsync(ExampleRequest request, CancellationToken cancellationToken)
        {
            var result = request.ValidationFailed == false ? R.Success : Reason.New<ValidationFailedReason>().Add("test", "notset");
            return Task.FromResult(result);
        }
    }

    [Fact]
    public async Task AsyncRequestWithoutResponseHandler_Success()
    {
        var result = await ExecutorFactory.CreateExecutor().ExecuteAsync(new ExampleRequest(), CancellationToken.None);

        Assert.Equal("aaaa", result);
    }

    [Fact]
    public async Task AsyncRequestWithoutResponseHandler_FailedWithReason()
    {
        var result = await ExecutorFactory.CreateExecutor().ExecuteAsync(new ExampleRequest(false), CancellationToken.None);

        Assert.True(result.Is<ReasonNotFound>());
    }
    [Fact]
    public async Task AsyncRequestWithoutResponseHandler_FailedOnValidation()
    {
        var result = await ExecutorFactory.CreateExecutor().ExecuteAsync(new ExampleRequest(false, true), CancellationToken.None);

        Assert.True(result.Is<ValidationFailedReason>());
    }
}
