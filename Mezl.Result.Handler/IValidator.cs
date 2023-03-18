namespace Mezl.Result.Handler;

public interface IValidator<in TRequest> where TRequest : IRequest
{
    R Validate(TRequest request);
}