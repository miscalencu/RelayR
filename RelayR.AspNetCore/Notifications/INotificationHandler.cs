namespace RelayR.AspNetCore.Notifications;

public interface INotificationHandler<T> where T : INotification
{
    Task HandleAsync(T message, CancellationToken cancellationToken);
}