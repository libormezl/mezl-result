using System.Reflection;

namespace Mezl.Result.Handler;

internal class RequestExecutor : IRequestExecutor
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HandlersCache _handlersCache;
    private readonly ValidatorsCache _validatorsCache;
    private readonly Dictionary<Type, FastMethodInfo> _handlersMethodCache = new();
    private readonly Dictionary<Type, FastMethodInfo> _validatorsMethodCache = new();

    public RequestExecutor(IServiceProvider serviceProvider, HandlersCache handlersCache, ValidatorsCache validatorsCache)
    {
        _serviceProvider = serviceProvider;
        _handlersCache = handlersCache;
        _validatorsCache = validatorsCache;
    }

    public async Task<R<TResponse>> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var validationResult = await ValidationResultReason(request, cancellationToken, requestType);
        if (validationResult.IsNotSuccessful)
        {
            return validationResult.Reason;
        }

        var (handlerObject, handler) = HandlerObject(requestType);
        var result = await (handler!.Invoke(handlerObject, request, cancellationToken) as Task<R<TResponse>>)!;
        return result;
    }

    public async Task<R> ExecuteAsync(IRequest request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var validationResult = await ValidationResultReason(request, cancellationToken, requestType);
        if (validationResult.IsNotSuccessful)
        {
            return validationResult.Reason;
        }

        var (handlerObject, handler) = HandlerObject(requestType);
        var result = await (handler!.Invoke(handlerObject, request, cancellationToken) as Task<R>)!;
        return result;
    }

    private (object handlerObject, FastMethodInfo handler) HandlerObject(Type requestType)
    {
        if (_handlersCache.TryGetValue(requestType, out var handlerType) == false)
        {
            throw new InvalidOperationException($"Handler for '{requestType.Name}' is not registered");
        }

        var handlerObject = _serviceProvider.GetService(handlerType)
                            ?? throw new InvalidOperationException($"Service for '{requestType.Name}' is not registered");

        var handler = FastMethodInfo(handlerType, requestType, _handlersMethodCache, "HandleAsync", true)!;
        return (handlerObject, handler);
    }

    public async void Notify(INotification request)
    {
        var requestType = request.GetType();

        if (_handlersCache.TryGetValue(requestType, out var handlerType) == false)
        {
            throw new InvalidOperationException($"Handler for '{requestType.Name}' is not registered");
        }

        var handlerObject = _serviceProvider.GetService(handlerType)
                            ?? throw new InvalidOperationException($"Service for '{requestType.Name}' is not registered");

        var handler = FastMethodInfo(handlerType, requestType, _handlersMethodCache, "HandleAsync", true);

        await (handler!.Invoke(handlerObject, request, CancellationToken.None) as Task)!;
    }

    private async Task<R> ValidationResultReason(object request, CancellationToken cancellationToken, Type requestType)
    {
        if (_validatorsCache.TryGetValue(requestType, out var validatorType) == false)
        {
            return R.Success;
        }

        var validator = FastMethodInfo(validatorType, requestType, _validatorsMethodCache, "ValidateAsync", false);
        if (validator != null)
        {
            var validatorObject = _serviceProvider.GetService(validatorType)
                                  ?? throw new InvalidOperationException(
                                      $"Service for '{requestType.Name}' is not registered");

            return await (validator.Invoke(validatorObject, request, cancellationToken) as Task<R>)!;
        }

        validator = FastMethodInfo(validatorType, requestType, _validatorsMethodCache, "Validate", false);
        if (validator != null)
        {
            var validatorObject = _serviceProvider.GetService(validatorType)
                                  ?? throw new InvalidOperationException(
                                      $"Service for '{requestType.Name}' is not registered");

            return (validator.Invoke(validatorObject, request) as R)!;
        }

        return R.Success;
    }

    private static FastMethodInfo? FastMethodInfo(Type handlerType, MemberInfo requestType, IDictionary<Type, FastMethodInfo> cache, string name, bool throwOnNull)
    {
        if (cache.TryGetValue(handlerType, out var handleInfo) == false)
        {
            var method = handlerType.GetMethod(name);
            if (method == null && throwOnNull)
            {
                throw new InvalidCastException($"'{requestType.Name}' does not contains '{name}' method");
            }

            if (method == null)
            {
                return null;
            }

            handleInfo = cache[handlerType] = new FastMethodInfo(method);
        }

        return handleInfo;
    }
}