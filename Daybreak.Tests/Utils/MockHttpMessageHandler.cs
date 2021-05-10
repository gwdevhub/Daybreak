using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Tests.Utils
{
    public sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        public Func<HttpRequestMessage, HttpResponseMessage> ResponseResolver { get; set; }
        public Func<HttpRequestMessage, Task<HttpResponseMessage>> ResponseResolverAsync { get; set; }

        public MockHttpMessageHandler()
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this.ResponseResolver is not null)
            {
                return Task.FromResult(this.ResponseResolver(request));
            }
            else
            {
                return this.ResponseResolverAsync(request);
            }
        }
    }
}
