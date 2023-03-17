namespace Mezl.Result.Handler;

public interface IRequestExecutor
{
    Task<R<TResponse>> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);

    Task<R> ExecuteAsync(IRequest request, CancellationToken cancellationToken);

    void ExecuteAsync(INotification request);
}