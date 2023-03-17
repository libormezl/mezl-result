namespace Mezl.Result.Handler;

public interface IAsyncNotificationHandler<in TNotification> where TNotification : INotification
{
    Task HandleAsync(TNotification request, CancellationToken cancellationToken);
}