using Microsoft.Extensions.Hosting;
using RelayR.AspNetCore.Core;

namespace RelayR.AspNetCore.Requests;

public class RequestsConsumer(IMediator mediator, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in mediator.RequestChannelReader.ReadAllAsync(stoppingToken))
        {
            var messageType = message.GetType();

            if (messageType.IsGenericType &&
                messageType.GetGenericTypeDefinition() == typeof(RequestWrapper<,>))
            {
                var request = message;
                var requestType = messageType.GetGenericArguments()[0]; var implementingRequestTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => p.GetInterfaces().Any(i => i.IsGenericType && i == requestType));
                if (!implementingRequestTypes.Any())
                {
                    throw new InvalidOperationException($"No implementing types found for {requestType}");
                }

                var responseType = messageType.GetGenericArguments()[1];

                var handlerType = typeof(IRequestHandler<,>).MakeGenericType(implementingRequestTypes.First(), responseType);
                var handler = serviceProvider.GetService(handlerType);

                if (handler != null)
                {
                    var requestProp = messageType.GetProperty("Request");
                    var tcsProp = messageType.GetProperty("Tcs");

                    var requestValue = requestProp.GetValue(request);
                    var tcs = tcsProp.GetValue(request);

                    var method = handlerType.GetMethod("HandleAsync");

                    try
                    {
                        var responseTask = (Task)method.Invoke(handler, new[] { requestValue, stoppingToken });
                        await responseTask;

                        var resultProp = responseTask.GetType().GetProperty("Result");
                        var result = resultProp.GetValue(responseTask);

                        var setResultMethod = tcs.GetType().GetMethod("SetResult");
                        setResultMethod.Invoke(tcs, new[] { result });
                    }
                    catch (Exception ex)
                    {
                        var setExceptionMethod = tcs.GetType().GetMethod("SetException", new[] { typeof(Exception) });
                        setExceptionMethod.Invoke(tcs, new[] { ex });
                    }
                }
            }
        }
    }
}