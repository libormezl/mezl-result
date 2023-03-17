namespace Mezl.Result.Handler;

public interface IAsyncRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<R<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

public interface IAsyncRequestHandler<in TRequest> where TRequest : IRequest
{
    Task<R> HandleAsync(TRequest request, CancellationToken cancellationToken);
}