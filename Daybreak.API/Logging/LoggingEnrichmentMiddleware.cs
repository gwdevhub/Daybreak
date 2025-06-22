using Microsoft.CorrelationVector;
using Microsoft.Extensions.Options;
using Net.Sdk.Web;
using Net.Sdk.Web.Options;
using Serilog.Context;

namespace Daybreak.API.Logging;

public sealed class LoggingEnrichmentMiddleware(IOptions<CorrelationVectorOptions> options)
    : IMiddleware
{
    private readonly CorrelationVectorOptions options = options.Value;

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.TryGetValue(this.options.Header, out var value) ||
            value.FirstOrDefault() is not string cvString ||
            CorrelationVector.Parse(cvString) is not CorrelationVector cv)
        {
            cv = new CorrelationVector();
        }

        cv = CorrelationVector.Extend(cv.ToString());
        context.SetCorrelationVector(cv);
        LogContext.PushProperty("CorrelationVector", cv);
        HeaderDictionaryExtensions.Append(context.Response.Headers, this.options.Header, context.GetCorrelationVector().ToString());
        return next(context);
    }
}
