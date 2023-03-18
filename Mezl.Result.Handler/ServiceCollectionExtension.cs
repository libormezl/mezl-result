using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Mezl.Result.Handler;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHandlers(this IServiceCollection collection)
    {
        var handlersCache = new HandlersCache();
        var validatorsCache = new ValidatorsCache();

        var handlers = new[]
        {
            typeof(IAsyncRequestHandler<,>),
            typeof(IAsyncRequestHandler<>),

            typeof(IAsyncNotificationHandler<>)
        };

        var validators = new[]
        {
            typeof(IAsyncValidator<>),
            typeof(IValidator<>)
        };

        var lifecycleAttributeType = typeof(LifecycleAttribute);

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                var handlerType = type
                    .GetInterfaces()
                    .FirstOrDefault(x => x.IsGenericType && handlers.Contains(x.GetGenericTypeDefinition()));

                var validatorType = type
                    .GetInterfaces()
                    .FirstOrDefault(x => x.IsGenericType && validators.Contains(x.GetGenericTypeDefinition()));

                if (handlerType == null && validatorType == null)
                {
                    continue;
                }

                if (type.GetCustomAttribute(lifecycleAttributeType, true) is LifecycleAttribute lifecycle)
                {
                    switch (lifecycle.Lifecycle)
                    {
                        case Lifecycle.Transient:
                            collection.AddTransient(type);
                            break;
                        case Lifecycle.Singleton:
                            collection.AddSingleton(type);
                            break;
                        case Lifecycle.Scoped:
                            collection.AddScoped(type);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    collection.AddTransient(type, type);
                }

                if (validatorType != null)
                {
                    validatorsCache.TryAdd(validatorType.GenericTypeArguments[0], type);
                }
                else if (handlerType != null)
                {
                    handlersCache.TryAdd(handlerType.GenericTypeArguments[0], type);
                }
            }
        }

        collection.AddSingleton<IRequestExecutor, RequestExecutor>();
        collection.AddSingleton(handlersCache);
        collection.AddSingleton(validatorsCache);
        return collection;
    }
}