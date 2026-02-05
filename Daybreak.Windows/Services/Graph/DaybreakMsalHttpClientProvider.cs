using Microsoft.Identity.Client;

namespace Daybreak.Windows.Services.Graph;

internal sealed class DaybreakMsalHttpClientProvider(HttpClient httpClient)
    : IMsalHttpClientFactory
{
    private readonly HttpClient httpClient = httpClient;

    public HttpClient GetHttpClient()
    {
        return this.httpClient;
    }
}
