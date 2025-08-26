using Microsoft.Extensions.DependencyInjection;
using RelayR.AspNetCore.Configuration;
using RelayR.AspNetCore.Core;
using RelayR.AspNetCore.Notifications;
using RelayR.AspNetCore.Requests;
using System.Reflection;

namespace RelayR.AspNetCore.Extensions;

public static class WebApplicationExtensions
{
    private static readonly RelayRServiceConfiguration configuration = new();

    public static IServiceCollection AddRelayR(this IServiceCollection services, Action<RelayRServiceConfiguration>? action = null)
    {
        action?.Invoke(configuration);
        return services.AddMediator();
    }

    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator>();
        services.RegisterTypes(typeof(INotificationHandler<>), configuration.AssembliesToRegister);
        services.RegisterTypes(typeof(IRequestHandler<,>), configuration.AssembliesToRegister);
        services.AddHostedService<NotificationsConsumer>();
        services.AddHostedService<RequestsConsumer>();
        return services;
    }

    private static IServiceCollection RegisterTypes(this IServiceCollection services, Type handlerInterfaceType, List<Assembly> assembliesToRegister)
    {
        var handlerTypes = assembliesToRegister
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Select(type => new
            {
                ImplementationType = type,
                HandlerInterfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
            })
            .Where(x => x.HandlerInterfaces.Any());

        foreach (var typeInfo in handlerTypes)
        {
            foreach (var handlerInterface in typeInfo.HandlerInterfaces)
            {
                services.AddTransient(handlerInterface, typeInfo.ImplementationType);
            }
        }

        return services;
    }
}