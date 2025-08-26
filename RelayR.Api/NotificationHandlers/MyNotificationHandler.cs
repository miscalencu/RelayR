using RelayR.Api.Notifications;
using RelayR.AspNetCore.Notifications;

namespace RelayR.Api.NotificationHandlers;

public class MyNotificationHandler : INotificationHandler<MyNotification>
{
    public Task HandleAsync(MyNotification message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Notification Handled: {message.Info} - {DateTime.Now}");
        return Task.CompletedTask;
    }
}