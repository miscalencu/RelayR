using System.Threading.Channels;

namespace RelayR.AspNetCore.Requests;

public interface ISender
{
    ChannelReader<object> RequestChannelReader { get; }

    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}