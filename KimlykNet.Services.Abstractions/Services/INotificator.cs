using System.Threading;
using System.Threading.Tasks;

namespace KimlykNet.Services.Abstractions.Services;

public interface INotificator
{
    Task NotifyAsync(ApplicationNotification message, CancellationToken cancellationToken = default);

    Task NotifyAuthenticationRequestedAsync(string email, CancellationToken cancellationToken = default);
    
    Task NotifyAuthenticationFailedAsync(string email, CancellationToken cancellationToken = default);
    
    Task NotifyAuthenticationSucceedAsync(string email, CancellationToken cancellationToken = default);
}