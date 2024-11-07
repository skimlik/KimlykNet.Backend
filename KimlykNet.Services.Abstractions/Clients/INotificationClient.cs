using System.Threading;
using System.Threading.Tasks;

using KimlykNet.Services.Abstractions.Services;

namespace KimlykNet.Services.Abstractions.Clients
{
    public interface INotificationClient
    {
        Task<bool> SendNotificationAsync(ApplicationNotification body, CancellationToken cancellationToken = default);
    }
}