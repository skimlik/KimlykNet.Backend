using System.Threading;
using System.Threading.Tasks;

namespace KimlykNet.Services.Abstractions.Services;

public interface INotificator
{
    Task NotifyAsync(string message, CancellationToken cancellationToken = default);
}