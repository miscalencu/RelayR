using RelayR.Api.Models;
using RelayR.Api.Requests;
using RelayR.AspNetCore.Requests;

namespace RelayR.Api.RequestHandlers;

public class MyRequestHandler : IRequestHandler<MyRequest, MyRequestResponse>
{
    public Task<MyRequestResponse> HandleAsync(MyRequest message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Request Handled: {message.Info} - {DateTime.Now}");
        return Task.FromResult(new MyRequestResponse($"Handled: {message.Info} - {DateTime.Now}"));
    }
}