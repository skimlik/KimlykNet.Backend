using System.Threading.Channels;

using KimlykNet.Services.Abstractions.Services;

namespace KimlykNet.Backend.Background;

public class NotificationChannelConsumer(
    Channel<ApplicationNotification> channel,
    INotificator notificator)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var reader = channel.Reader;
        while (!channel.Reader.Completion.IsCanceled && await reader.WaitToReadAsync(stoppingToken))
        {
            if (reader.TryRead(out var msg))
            {
                await notificator.NotifyAsync(msg, stoppingToken);
            }
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        channel.Writer.TryComplete();
        return base.StopAsync(cancellationToken);
    }
}