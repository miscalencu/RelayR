using System.Threading.Channels;

namespace RelayR.AspNetCore.Notifications;

public interface IPublisher
{
    ChannelReader<object> EventChannelReader { get; }

    Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification;
}