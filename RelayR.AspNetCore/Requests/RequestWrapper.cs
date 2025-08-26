namespace RelayR.AspNetCore.Requests;

public class RequestWrapper<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>
{
    public TRequest Request { get; } = request;

    public TaskCompletionSource<TResponse> Tcs { get; } =
        new TaskCompletionSource<TResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
}