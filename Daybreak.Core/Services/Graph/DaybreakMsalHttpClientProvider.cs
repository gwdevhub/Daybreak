using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace Daybreak.Services.Graph;

/// <summary>
/// Custom HTTP client factory for MSAL that uses the application's logging infrastructure.
/// This ensures MSAL HTTP calls are logged consistently with the rest of the application.
/// </summary>
internal sealed class DaybreakMsalHttpClientProvider : IMsalHttpClientFactory
{
    private readonly HttpClient httpClient;

    public DaybreakMsalHttpClientProvider(ILogger<GraphClient> logger)
    {
        var handler = new LoggingHttpMessageHandler(logger)
        {
            InnerHandler = new HttpClientHandler()
        };

        this.httpClient = new HttpClient(handler);
    }

    public HttpClient GetHttpClient()
    {
        return this.httpClient;
    }
}
