using RelayR.AspNetCore.Notifications;
using RelayR.AspNetCore.Requests;
using System.Threading.Channels;

namespace RelayR.AspNetCore.Core;

public class Mediator : IMediator
{
    private readonly Channel<object> _eventChannel = Channel.CreateUnbounded<object>();
    public ChannelReader<object> EventChannelReader => _eventChannel.Reader;

    private readonly Channel<object> _requestChannel = Channel.CreateUnbounded<object>();
    public ChannelReader<object> RequestChannelReader => _requestChannel.Reader;

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        await _eventChannel.Writer.WriteAsync(notification, cancellationToken);
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var wrapper = new RequestWrapper<IRequest<TResponse>, TResponse>(request);
        if (!_requestChannel.Writer.TryWrite(wrapper))
        {
            throw new InvalidOperationException("Failed to write to the request channel.");
        }
        return wrapper.Tcs.Task;
    }
}