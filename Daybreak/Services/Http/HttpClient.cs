using Daybreak.Services.Logging;
using Daybreak.Utils;
using System;
using System.Extensions;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Http
{
    public sealed class HttpClient<Tscope> : IHttpClient<Tscope>, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;
        private readonly string scope;

        public Uri BaseAddress { get => this.httpClient.BaseAddress; set => this.httpClient.BaseAddress = value; }
        public HttpRequestHeaders DefaultRequestHeaders => this.httpClient.DefaultRequestHeaders;
        public Version DefaultRequestVersion { get => this.httpClient.DefaultRequestVersion; set => this.httpClient.DefaultRequestVersion = value; }
        public HttpVersionPolicy DefaultVersionPolicy { get => this.httpClient.DefaultVersionPolicy; set => this.httpClient.DefaultVersionPolicy = value; }
        public long MaxResponseContentBufferSize { get => this.httpClient.MaxResponseContentBufferSize; set => this.httpClient.MaxResponseContentBufferSize = value; }
        public TimeSpan Timeout { get => this.httpClient.Timeout; set => this.httpClient.Timeout = value; }

        public HttpClient(
            ILogger logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.httpClient = new HttpClient();
            this.scope = typeof(Tscope).Name;
            this.OnClientInitialized();
        }

        public HttpClient(
            HttpMessageHandler handler,
            ILogger logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.httpClient = new HttpClient(handler);
            this.scope = typeof(Tscope).Name;
            this.OnClientInitialized();
        }

        public HttpClient(
            HttpMessageHandler handler,
            bool disposeHandler,
            ILogger logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.httpClient = new HttpClient(handler, disposeHandler);
            this.scope = typeof(Tscope).Name;
            this.OnClientInitialized();
        }

        public void CancelPendingRequests()
        {
            this.LogInformation("Canceling request");
            this.httpClient.CancelPendingRequests();
        }
        public Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            this.LogInformation($"DeleteAsync {requestUri}");
            return this.httpClient.DeleteAsync(requestUri);
        }
        public Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
        {
            this.LogInformation($"DeleteAsync {requestUri}");
            return this.httpClient.DeleteAsync(requestUri, cancellationToken);
        }
        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            this.LogInformation($"DeleteAsync {requestUri}");
            return this.httpClient.DeleteAsync(requestUri);
        }
        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            this.LogInformation($"DeleteAsync {requestUri}");
            return this.httpClient.DeleteAsync(requestUri, cancellationToken);
        }
        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            this.LogInformation($"GetAsync {requestUri}");
            return this.httpClient.GetAsync(requestUri);
        }
        public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)
        {
            this.LogInformation($"GetAsync {requestUri}");
            return this.httpClient.GetAsync(requestUri, completionOption);
        }
        public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            this.LogInformation($"GetAsync {requestUri}");
            return this.httpClient.GetAsync(requestUri, completionOption, cancellationToken);
        }
        public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            this.LogInformation($"GetAsync {requestUri}");
            return this.httpClient.GetAsync(requestUri, cancellationToken);
        }
        public Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            this.LogInformation($"GetAsync {requestUri}");
            return this.httpClient.GetAsync(requestUri);
        }
        public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
        {
            this.LogInformation($"GetAsync {requestUri}");
            return this.httpClient.GetAsync(requestUri, completionOption);
        }
        public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            this.LogInformation($"GetAsync {requestUri}");
            return this.httpClient.GetAsync(requestUri, completionOption, cancellationToken);
        }
        public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            this.LogInformation($"GetAsync {requestUri}");
            return this.httpClient.GetAsync(requestUri, cancellationToken);
        }
        public Task<byte[]> GetByteArrayAsync(string requestUri)
        {
            this.LogInformation($"GetByteArrayAsync {requestUri}");
            return this.httpClient.GetByteArrayAsync(requestUri);
        }
        public Task<byte[]> GetByteArrayAsync(string requestUri, CancellationToken cancellationToken)
        {
            this.LogInformation($"GetByteArrayAsync {requestUri}");
            return this.httpClient.GetByteArrayAsync(requestUri, cancellationToken);
        }
        public Task<byte[]> GetByteArrayAsync(Uri requestUri)
        {
            this.LogInformation($"GetByteArrayAsync {requestUri}");
            return this.httpClient.GetByteArrayAsync(requestUri);
        }
        public Task<byte[]> GetByteArrayAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            this.LogInformation($"GetByteArrayAsync {requestUri}");
            return this.httpClient.GetByteArrayAsync(requestUri, cancellationToken);
        }
        public Task<Stream> GetStreamAsync(string requestUri)
        {
            this.LogInformation($"GetStreamAsync {requestUri}");
            return this.httpClient.GetStreamAsync(requestUri);
        }
        public Task<Stream> GetStreamAsync(string requestUri, CancellationToken cancellationToken)
        {
            this.LogInformation($"GetStreamAsync {requestUri}");
            return this.httpClient.GetStreamAsync(requestUri, cancellationToken);
        }
        public Task<Stream> GetStreamAsync(Uri requestUri)
        {
            this.LogInformation($"GetStreamAsync {requestUri}");
            return this.httpClient.GetStreamAsync(requestUri);
        }
        public Task<Stream> GetStreamAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            this.LogInformation($"GetStreamAsync {requestUri}");
            return this.httpClient.GetStreamAsync(requestUri, cancellationToken);
        }
        public Task<string> GetStringAsync(string requestUri)
        {
            this.LogInformation($"GetStringAsync {requestUri}");
            return this.httpClient.GetStringAsync(requestUri);
        }
        public Task<string> GetStringAsync(string requestUri, CancellationToken cancellationToken)
        {
            this.LogInformation($"GetStringAsync {requestUri}");
            return this.httpClient.GetStringAsync(requestUri, cancellationToken);
        }
        public Task<string> GetStringAsync(Uri requestUri)
        {
            this.LogInformation($"GetStringAsync {requestUri}");
            return this.httpClient.GetStringAsync(requestUri);
        }
        public Task<string> GetStringAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            this.LogInformation($"GetStringAsync {requestUri}");
            return this.httpClient.GetStringAsync(requestUri, cancellationToken);
        }
        public Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content)
        {
            this.LogInformation($"PatchAsync {requestUri}");
            return this.httpClient.PatchAsync(requestUri, content);
        }
        public Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            this.LogInformation($"PatchAsync {requestUri}");
            return this.httpClient.PatchAsync(requestUri, content, cancellationToken);
        }
        public Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content)
        {
            this.LogInformation($"PatchAsync {requestUri}");
            return this.httpClient.PatchAsync(requestUri, content);
        }
        public Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            this.LogInformation($"PatchAsync {requestUri}");
            return this.httpClient.PatchAsync(requestUri, content, cancellationToken);
        }
        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            this.LogInformation($"PatchAsync {requestUri}");
            return this.httpClient.PostAsync(requestUri, content);
        }
        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            this.LogInformation($"PostAsync {requestUri}");
            return this.httpClient.PostAsync(requestUri, content, cancellationToken);
        }
        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            this.LogInformation($"PostAsync {requestUri}");
            return this.httpClient.PostAsync(requestUri, content);
        }
        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            this.LogInformation($"PostAsync {requestUri}");
            return this.httpClient.PostAsync(requestUri, content, cancellationToken);
        }
        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            this.LogInformation($"PutAsync {requestUri}");
            return this.httpClient.PutAsync(requestUri, content);
        }
        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            this.LogInformation($"PutAsync {requestUri}");
            return this.httpClient.PutAsync(requestUri, content, cancellationToken);
        }
        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            this.LogInformation($"PutAsync {requestUri}");
            return this.httpClient.PutAsync(requestUri, content);
        }
        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            this.LogInformation($"PutAsync {requestUri}");
            return this.httpClient.PutAsync(requestUri, content, cancellationToken);
        }
        public HttpResponseMessage Send(HttpRequestMessage request)
        {
            this.LogInformation($"Send {request.RequestUri}");
            return this.httpClient.Send(request);
        }
        public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            this.LogInformation($"Send {request.RequestUri}");
            return this.httpClient.Send(request, completionOption);
        }
        public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            this.LogInformation($"Send {request.RequestUri}");
            return this.httpClient.Send(request, completionOption, cancellationToken);
        }
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            this.LogInformation($"SendAsync {request.RequestUri}");
            return this.httpClient.SendAsync(request);
        }
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            this.LogInformation($"SendAsync {request.RequestUri}");
            return this.httpClient.SendAsync(request, completionOption);
        }
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            this.LogInformation($"SendAsync {request.RequestUri}");
            return this.httpClient.SendAsync(request, completionOption, cancellationToken);
        }
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.LogInformation($"SendAsync {request.RequestUri}");
            return this.httpClient.SendAsync(request, cancellationToken);
        }
        public void Dispose()
        {
            this.httpClient.Dispose();
        }

        private void OnClientInitialized()
        {
            this.logger.LogInformation($"Initialized scoped http client for {this.scope}");
        }
        private void LogInformation(string message)
        {
            this.logger.LogInformation($"{this.scope} - {message}");
        }
    }
}
