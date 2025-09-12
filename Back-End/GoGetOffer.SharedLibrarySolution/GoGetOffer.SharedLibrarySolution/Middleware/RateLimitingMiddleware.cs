using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System.Text.Json;

namespace GoGetOffer.SharedLibrarySolution.Middleware
{
    public class RateLimitingMiddleware(RequestDelegate next, IConnectionMultiplexer redis)
    {
        private readonly RequestDelegate _next = next;
        private readonly IDatabase _redisDb = redis.GetDatabase();

        private readonly int _limit = 20;
        private readonly int _windowSeconds = 30;

        public async Task InvokeAsync(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var key = $"rate_limit:{ipAddress}";
            var count = await _redisDb.StringIncrementAsync(key);

            if (count == 1)
            {
                await _redisDb.KeyExpireAsync(key, TimeSpan.FromSeconds(_windowSeconds));
            }

            if (count > _limit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.ContentType = "application/json";
                var response = Response<string>.Failure("Too many requests. Please try again later.");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                await context.Response.CompleteAsync();
                return;
            }

            await _next(context);
        }
    }
}