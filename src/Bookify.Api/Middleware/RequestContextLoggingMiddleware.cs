using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace Bookify.Api.Middleware
{
    public class RequestContextLoggingMiddleware : IMiddleware
    {
        private const string CorrelationHeaderName = "X-Correlation-Id";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
            {
                await next(context);
            }
        }

        private static string GetCorrelationId(HttpContext context)
        {
            context.Request.Headers.TryGetValue(CorrelationHeaderName, out StringValues correlationId);
            return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
        }
    }
}