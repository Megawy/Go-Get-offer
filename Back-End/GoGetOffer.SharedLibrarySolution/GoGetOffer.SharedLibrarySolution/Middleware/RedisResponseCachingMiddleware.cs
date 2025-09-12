using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System.Text;

namespace GoGetOffer.SharedLibrarySolution.Middleware
{
    public class RedisResponseCachingMiddleware(RequestDelegate next, IConnectionMultiplexer redis)
    {
        private readonly RequestDelegate _next = next;
        private readonly IDatabase _redisDb = redis.GetDatabase();
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method != HttpMethods.Get)
            {
                await _next(context);
                return;
            }

            var cacheKey = GenerateCacheKey(context.Request);
            var cachedResponse = await _redisDb.StringGetAsync(cacheKey);

            if (cachedResponse.HasValue)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(cachedResponse.ToString());
                return;
            }

            var originalBodyStream = context.Response.Body;
            using var newBodyStream = new MemoryStream();
            context.Response.Body = newBodyStream;

            await _next(context);

            newBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();
            newBodyStream.Seek(0, SeekOrigin.Begin);

            // Cache in Redis for 2 minutes
            await _redisDb.StringSetAsync(cacheKey, responseBody, TimeSpan.FromMinutes(2));

            await newBodyStream.CopyToAsync(originalBodyStream);
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}