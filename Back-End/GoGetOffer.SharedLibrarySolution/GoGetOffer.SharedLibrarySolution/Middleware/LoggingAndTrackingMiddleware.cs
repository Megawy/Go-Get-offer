using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GoGetOffer.SharedLibrarySolution.Middleware
{
    public class LoggingAndTrackingMiddleware(RequestDelegate next, ILogger<LoggingAndTrackingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<LoggingAndTrackingMiddleware> _logger = logger;

        // Extension Method
        public async Task InvokeAsync(HttpContext context)
        {
            // 1) Generate RequestId (CorrelationId)
            var requestId = Guid.NewGuid().ToString();
            context.Items["RequestId"] = requestId;
            context.Response.Headers["X-Request-Id"] = requestId;

            var stopwatch = Stopwatch.StartNew();

            try
            {
                // 2) Log Incoming Request
                _logger.LogInformation("➡️ Request {RequestId}: {Method} {Path}",
                    requestId, context.Request.Method, context.Request.Path);

                await _next(context); // pass to next middleware

                stopwatch.Stop();

                // 3) Log Outgoing Response
                _logger.LogInformation("⬅️ Response {RequestId}: {StatusCode} in {Elapsed}ms",
                    requestId, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                // 4) Log Exception (if not caught by ExceptionMiddleware)
                _logger.LogError(ex, "❌ Error {RequestId} in {Elapsed}ms",
                    requestId, stopwatch.ElapsedMilliseconds);

                throw; // rethrow to be handled by Exception Middleware
            }
        }
    }
}