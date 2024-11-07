using System.Threading.Channels;

using KimlykNet.Services.Abstractions.Services;

namespace KimlykNet.Services;

public class NotificationChannel(Channel<ApplicationNotification> channel)
{
    public bool TryWrite(ApplicationNotification notification)
    {
        return channel.Writer.TryWrite(notification);
    }

    public async ValueTask WriteAsync(ApplicationNotification notification, CancellationToken cancellationToken = default)
    {
        while (await channel.Writer.WaitToWriteAsync(cancellationToken))
        {
            await channel.Writer.WriteAsync(notification, cancellationToken);
            break;
        }
    }

    public void ScheduleNotification(ApplicationNotification notification, CancellationToken cancellationToken = default)
    {
        Task.Factory.StartNew(
            async () => await WriteAsync(notification, cancellationToken),
            cancellationToken);
    }
}