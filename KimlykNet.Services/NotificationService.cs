using KimlykNet.Services.Abstractions.Clients;
using KimlykNet.Services.Abstractions.Services;

namespace KimlykNet.Services;

public class NotificationService(INotificationClient client) : INotificator
{
    public Task NotifyAsync(ApplicationNotification message, CancellationToken cancellationToken = default) =>
        client.SendNotificationAsync(message, cancellationToken);

    public Task NotifyAuthenticationRequestedAsync(string email, CancellationToken cancellationToken = default) =>
        client.SendNotificationAsync(
            new ApplicationNotification
            {
                Text = $"Authentication token requested: {email}"
            },
            cancellationToken);

    public Task NotifyAuthenticationFailedAsync(string email, CancellationToken cancellationToken = default) =>
        client.SendNotificationAsync(
            new ApplicationNotification
            {
                Text = $"Authentication token request rejected: {email}"
            },
            cancellationToken);

    public Task NotifyAuthenticationSucceedAsync(string email, CancellationToken cancellationToken = default) =>
        client.SendNotificationAsync(
            new ApplicationNotification
            {
                Text = $"Authentication token request succeed: {email}"
            },
            cancellationToken);
}