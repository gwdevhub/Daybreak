using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Http
{
    public interface IHttpClient<TScope>
    {
        Uri BaseAddress { get; set; }
        HttpRequestHeaders DefaultRequestHeaders { get; }
        Version DefaultRequestVersion { get; set; }
        HttpVersionPolicy DefaultVersionPolicy { get; set; }
        long MaxResponseContentBufferSize { get; set; }
        TimeSpan Timeout { get; set; }

        void CancelPendingRequests();
        Task<HttpResponseMessage> DeleteAsync(string requestUri);
        Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri);
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> GetAsync(string requestUri);
        Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption);
        Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> GetAsync(Uri requestUri);
        Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption);
        Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);
        Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken);
        Task<byte[]> GetByteArrayAsync(string requestUri);
        Task<byte[]> GetByteArrayAsync(string requestUri, CancellationToken cancellationToken);
        Task<byte[]> GetByteArrayAsync(Uri requestUri);
        Task<byte[]> GetByteArrayAsync(Uri requestUri, CancellationToken cancellationToken);
        Task<Stream> GetStreamAsync(string requestUri);
        Task<Stream> GetStreamAsync(string requestUri, CancellationToken cancellationToken);
        Task<Stream> GetStreamAsync(Uri requestUri);
        Task<Stream> GetStreamAsync(Uri requestUri, CancellationToken cancellationToken);
        Task<string> GetStringAsync(string requestUri);
        Task<string> GetStringAsync(string requestUri, CancellationToken cancellationToken);
        Task<string> GetStringAsync(Uri requestUri);
        Task<string> GetStringAsync(Uri requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content);
        Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content);
        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken);
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content);
        Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);
        [UnsupportedOSPlatform("browser")]
        HttpResponseMessage Send(HttpRequestMessage request);
        [UnsupportedOSPlatform("browser")]
        HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption);
        [UnsupportedOSPlatform("browser")]
        HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken);
        [UnsupportedOSPlatform("browser")]
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
