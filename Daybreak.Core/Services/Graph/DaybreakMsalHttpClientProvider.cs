using Microsoft.Identity.Client;

namespace Daybreak.Services.Graph;
internal sealed class DaybreakMsalHttpClientProvider(HttpClient httpClient)
    : IMsalHttpClientFactory
{
    private readonly HttpClient httpClient = httpClient;

    public HttpClient GetHttpClient()
    {
        return this.httpClient;
    }
}
