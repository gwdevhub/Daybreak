using Daybreak.Services.Http;
using Daybreak.Services.Logging;
using Daybreak.Tests.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Tests.Services
{
    [TestClass]
    public class HttpClientTests
    {
        private const string resourceUrl = "resource";

        private readonly Uri TestAddressUrl = new("https://helloworld.xyz");
        private readonly ILogger logger = new Mock<ILogger>().Object;
        private readonly MockHttpMessageHandler handler = new();
        private IHttpClient<object> httpClient;

        [TestInitialize]
        public void TestInitialize()
        {
            this.httpClient = new HttpClient<object>(handler, false, logger)
            {
                BaseAddress = TestAddressUrl
            };
        }

        [TestMethod]
        public void SetBaseAddress()
        {
            this.httpClient.BaseAddress = TestAddressUrl;
            this.httpClient.BaseAddress.Should().Be(TestAddressUrl);
        }
        [TestMethod]
        public void SetDefaultRequestHeaders()
        {
            this.httpClient.DefaultRequestHeaders.TryAddWithoutValidation("someheader", "thisvalue");
            this.httpClient.DefaultRequestHeaders.GetValues("someheader").Should().BeEquivalentTo("thisvalue");
        }
        [TestMethod]
        public void SetDefaultRequestVersion()
        {
            var desiredVersion = new Version(0, 0, 1);
            this.httpClient.DefaultRequestVersion = desiredVersion;
            this.httpClient.DefaultRequestVersion.Should().Be(desiredVersion);
        }
        [TestMethod]
        public void SetDefaultHttpVersionPolicy()
        {
            this.httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;
            this.httpClient.DefaultVersionPolicy.Should().Be(HttpVersionPolicy.RequestVersionExact);
        }
        [TestMethod]
        public void SetMaxResponseContentBufferSize()
        {
            this.httpClient.MaxResponseContentBufferSize = 2000;
            this.httpClient.MaxResponseContentBufferSize.Should().Be(2000);
        }
        [TestMethod]
        public void SetTimeout()
        {
            this.httpClient.Timeout = TimeSpan.FromSeconds(1);
            this.httpClient.Timeout.Should().Be(TimeSpan.FromSeconds(1));
        }
        [TestMethod]
        public async Task DeleteAsyncStringReturns200()
        {
            this.SetupResponse(new Uri(TestAddressUrl, resourceUrl), HttpStatusCode.OK);
            
            var response = await this.httpClient.DeleteAsync(resourceUrl);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task DeleteAsyncUriReturns200()
        {
            this.SetupResponse(TestAddressUrl, HttpStatusCode.OK);

            var response = await this.httpClient.DeleteAsync(TestAddressUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task DeleteAsyncCanceledThrowsTaskCanceledException()
        {
            this.SetupLongResponse(TestAddressUrl, HttpStatusCode.OK);
            var cts = new CancellationTokenSource();

            var responseTask = this.httpClient.DeleteAsync(TestAddressUrl, cts.Token);
            cts.Cancel();

            var awaitAction = new Func<Task<HttpResponseMessage>>(async () =>
            {
                return await responseTask;
            });

            await awaitAction.Should().ThrowAsync<TaskCanceledException>();
        }
        [TestMethod]
        public async Task GetAsyncStringReturns200()
        {
            this.SetupResponse(new Uri(TestAddressUrl, resourceUrl), HttpStatusCode.OK);

            var response = await this.httpClient.GetAsync(resourceUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task GetAsyncUriReturns200()
        {
            this.SetupResponse(TestAddressUrl, HttpStatusCode.OK);

            var response = await this.httpClient.GetAsync(TestAddressUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task GetAsyncCanceledThrowsTaskCanceledException()
        {
            this.SetupLongResponse(TestAddressUrl, HttpStatusCode.OK);
            var cts = new CancellationTokenSource();

            var responseTask = this.httpClient.GetAsync(TestAddressUrl, cts.Token);
            cts.Cancel();

            var awaitAction = new Func<Task<HttpResponseMessage>>(async () =>
            {
                return await responseTask;
            });

            await awaitAction.Should().ThrowAsync<TaskCanceledException>();
        }
        [TestMethod]
        public async Task PostAsyncStringReturns200()
        {
            this.SetupResponse(new Uri(TestAddressUrl, resourceUrl), HttpStatusCode.OK);

            var response = await this.httpClient.PostAsync(resourceUrl, null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task PostAsyncUriReturns200()
        {
            this.SetupResponse(TestAddressUrl, HttpStatusCode.OK);

            var response = await this.httpClient.PostAsync(TestAddressUrl, null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task PostAsyncCanceledThrowsTaskCanceledException()
        {
            this.SetupLongResponse(TestAddressUrl, HttpStatusCode.OK);
            var cts = new CancellationTokenSource();

            var responseTask = this.httpClient.PostAsync(TestAddressUrl, null, cts.Token);
            cts.Cancel();

            var awaitAction = new Func<Task<HttpResponseMessage>>(async () =>
            {
                return await responseTask;
            });

            await awaitAction.Should().ThrowAsync<TaskCanceledException>();
        }
        [TestMethod]
        public async Task PatchAsyncStringReturns200()
        {
            this.SetupResponse(new Uri(TestAddressUrl, resourceUrl), HttpStatusCode.OK);

            var response = await this.httpClient.PatchAsync(resourceUrl, null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task PatchAsyncUriReturns200()
        {
            this.SetupResponse(TestAddressUrl, HttpStatusCode.OK);

            var response = await this.httpClient.PatchAsync(TestAddressUrl, null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task PatchAsyncCanceledThrowsTaskCanceledException()
        {
            this.SetupLongResponse(TestAddressUrl, HttpStatusCode.OK);
            var cts = new CancellationTokenSource();

            var responseTask = this.httpClient.PatchAsync(TestAddressUrl, null, cts.Token);
            cts.Cancel();

            var awaitAction = new Func<Task<HttpResponseMessage>>(async () =>
            {
                return await responseTask;
            });

            await awaitAction.Should().ThrowAsync<TaskCanceledException>();
        }
        [TestMethod]
        public async Task SendAsyncUriReturns200()
        {
            this.SetupResponse(TestAddressUrl, HttpStatusCode.OK);

            var response = await this.httpClient.SendAsync(new HttpRequestMessage { RequestUri = TestAddressUrl });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [TestMethod]
        public async Task SendAsyncCanceledThrowsTaskCanceledException()
        {
            this.SetupLongResponse(TestAddressUrl, HttpStatusCode.OK);
            var cts = new CancellationTokenSource();

            var responseTask = this.httpClient.SendAsync(new HttpRequestMessage { RequestUri = TestAddressUrl }, cts.Token);
            cts.Cancel();

            var awaitAction = new Func<Task<HttpResponseMessage>>(async () =>
            {
                return await responseTask;
            });

            await awaitAction.Should().ThrowAsync<TaskCanceledException>();
        }
        [TestMethod]
        public async Task GetStreamAsyncStringReturns200()
        {
            using var ms = new MemoryStream(new byte[] { 13, 10, 11, 12 });
            using var sc = new StreamContent(ms);
            this.SetupResponse(new Uri(TestAddressUrl, resourceUrl), HttpStatusCode.OK, sc);

            var response = await this.httpClient.GetStreamAsync(resourceUrl);

            var responseBuffer = new byte[4];
            await response.ReadAsync(responseBuffer.AsMemory(0, 4));
            responseBuffer[0].Should().Be(13);
            responseBuffer[1].Should().Be(10);
            responseBuffer[2].Should().Be(11);
            responseBuffer[3].Should().Be(12);
        }
        [TestMethod]
        public async Task GetStreamAsyncUriReturns200()
        {
            using var ms = new MemoryStream(new byte[] { 13, 10, 11, 12 });
            using var sc = new StreamContent(ms);
            this.SetupResponse(TestAddressUrl, HttpStatusCode.OK, sc);

            var response = await this.httpClient.GetStreamAsync(TestAddressUrl);

            var responseBuffer = new byte[4];
            await response.ReadAsync(responseBuffer.AsMemory(0, 4));
            responseBuffer[0].Should().Be(13);
            responseBuffer[1].Should().Be(10);
            responseBuffer[2].Should().Be(11);
            responseBuffer[3].Should().Be(12);
        }
        [TestMethod]
        public async Task GetStringAsyncStringReturns200()
        {
            using var sc = new StringContent("hello");
            this.SetupResponse(new Uri(TestAddressUrl, resourceUrl), HttpStatusCode.OK, sc);

            var response = await this.httpClient.GetStringAsync(resourceUrl);

            response.Should().Be("hello");
        }
        [TestMethod]
        public async Task GetStringAsyncUriReturns200()
        {
            using var sc = new StringContent("hello");
            this.SetupResponse(TestAddressUrl, HttpStatusCode.OK, sc);

            var response = await this.httpClient.GetStringAsync(TestAddressUrl);

            response.Should().Be("hello");
        }
        [TestMethod]
        public async Task GetByteArrayAsyncStringReturns200()
        {
            using var sc = new ByteArrayContent(new byte[4] { 13, 10, 11, 12 });
            this.SetupResponse(new Uri(TestAddressUrl, resourceUrl), HttpStatusCode.OK, sc);

            var response = await this.httpClient.GetByteArrayAsync(resourceUrl);

            response[0].Should().Be(13);
            response[1].Should().Be(10);
            response[2].Should().Be(11);
            response[3].Should().Be(12);
        }
        [TestMethod]
        public async Task GetByteArrayAsyncUriReturns200()
        {
            using var sc = new ByteArrayContent(new byte[4] { 13, 10, 11, 12 });
            this.SetupResponse(TestAddressUrl, HttpStatusCode.OK, sc);

            var response = await this.httpClient.GetByteArrayAsync(TestAddressUrl);

            response[0].Should().Be(13);
            response[1].Should().Be(10);
            response[2].Should().Be(11);
            response[3].Should().Be(12);
        }
        [TestMethod]
        public async Task CancelPendingRequestThrowsTaskCanceledException()
        {
            this.SetupLongResponse(TestAddressUrl, HttpStatusCode.OK);

            var responseTask = this.httpClient.GetAsync(TestAddressUrl);
            this.httpClient.CancelPendingRequests();

            var awaitAction = new Func<Task<HttpResponseMessage>>(async () =>
            {
                return await responseTask;
            });

            await awaitAction.Should().ThrowAsync<TaskCanceledException>();
        }

        private void SetupResponse(Uri expectedUri, HttpStatusCode statusCode, HttpContent httpContent = null)
        {
            this.handler.ResponseResolver = (request) =>
            {
                request.RequestUri.Should().Be(expectedUri);
                return new HttpResponseMessage { StatusCode = statusCode, Content = httpContent };
            };
        }
        private void SetupLongResponse(Uri expectedUri, HttpStatusCode statusCode, HttpContent httpContent = null)
        {
            this.handler.ResponseResolverAsync = async (request) =>
            {
                request.RequestUri.Should().Be(expectedUri);
                for(int i = 0; i < 10; i++)
                {
                    await Task.Delay(100);
                }

                return new HttpResponseMessage { StatusCode = statusCode, Content = httpContent };
            };
        }
    }
}
