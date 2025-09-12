using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace GoGetOffer.SharedLibrarySolution.Middleware
{
    public class SerilogRequestIdMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = context.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();

            context.Response.Headers["X-Request-Id"] = requestId;

            using (LogContext.PushProperty("RequestId", requestId))
            {
                await _next(context);
            }
        }
    }
}
