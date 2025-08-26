using Microsoft.Extensions.Hosting;
using RelayR.AspNetCore.Core;

namespace RelayR.AspNetCore.Notifications;

public class NotificationsConsumer(IMediator mediator, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in mediator.EventChannelReader.ReadAllAsync(stoppingToken))
        {
            var messageType = message.GetType();
            var handlerType = typeof(INotificationHandler<>).MakeGenericType(messageType);
            var handler = serviceProvider.GetService(handlerType);

            if (handler != null)
            {
                var method = handlerType.GetMethod("HandleAsync");
                await (Task)method.Invoke(handler, new[] { message, stoppingToken });
            }
        }
    }
}