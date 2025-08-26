using RelayR.AspNetCore.Notifications;

namespace RelayR.Api.Notifications;
public record MyNotification(string Info) : INotification;