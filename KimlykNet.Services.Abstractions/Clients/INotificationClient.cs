using System.Threading;
using System.Threading.Tasks;

namespace KimlykNet.Services.Abstractions.Clients
{
    public interface INotificationClient
    {
        Task SendNotificationAsync(string body, CancellationToken cancellationToken = default);
    }
}