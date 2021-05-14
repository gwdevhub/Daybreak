using Microsoft.Extensions.Logging;
using System;
using System.Extensions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Utils
{
    public sealed class LoggingHttpHandler : DelegatingHandler
    {
        private readonly ILogger logger;

        public LoggingHttpHandler(ILogger logger, HttpMessageHandler innerHandler) : base(innerHandler)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"{request.Method} - {request.RequestUri}");
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                this.logger.LogInformation($"{request.RequestUri} - {response.StatusCode}");
                return response;
            }
            catch(Exception e)
            {
                this.logger.LogError($"Exception encountered while processing {request.Method} - {request.RequestUri}", e);
                throw;
            }
        }
    }
}
