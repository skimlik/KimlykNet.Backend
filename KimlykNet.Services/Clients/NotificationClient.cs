using System.Text;
using System.Text.Json;

using KimlykNet.Services.Abstractions.Clients;
using KimlykNet.Services.Abstractions.Configuration;
using KimlykNet.Services.Abstractions.Services;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KimlykNet.Services.Clients;

public class NotificationClient(
    IOptions<NotificationsSettings> settings,
    HttpClient httpClient,
    JsonSerializerOptions serializerOptions,
    ILogger<NotificationClient> logger)
    : INotificationClient
{
    public async Task<bool> SendNotificationAsync(ApplicationNotification body, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = JsonSerializer.Serialize(body, serializerOptions);
            var uri = $"{settings.Value.WebHookUri}&payload={payload}";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            requestMessage.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(requestMessage, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Error sending notification");
            }

            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error sending notification");
            return false;
        }
    }
}