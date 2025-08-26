namespace RelayR.AspNetCore.Requests;

public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest message, CancellationToken cancellationToken);
}