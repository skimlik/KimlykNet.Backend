using System.Text;
using System.Text.Json;

using KimlykNet.Services.Abstractions.Clients;
using KimlykNet.Services.Abstractions.Configuration;

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
    private readonly ILogger<NotificationClient> _logger = logger;

    public async Task SendNotificationAsync(string body, CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(new { text = body }, serializerOptions);
        var uri = $"{settings.Value.WebHookUri}&payload={payload}";

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
        requestMessage.Content = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await httpClient.SendAsync(requestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}