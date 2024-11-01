using KimlykNet.Services.Abstractions.Clients;
using KimlykNet.Services.Abstractions.Services;

namespace KimlykNet.Services;

public class NotificationService(INotificationClient client) : INotificator
{
    public Task NotifyAsync(string message, CancellationToken cancellationToken = default) =>
        client.SendNotificationAsync(message, cancellationToken);
}