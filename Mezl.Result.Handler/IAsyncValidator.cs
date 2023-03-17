namespace Mezl.Result.Handler;

public interface IAsyncValidator<in TRequest> where TRequest : IRequest
{
    Task<R> ValidateAsync(TRequest request, CancellationToken cancellationToken);
}