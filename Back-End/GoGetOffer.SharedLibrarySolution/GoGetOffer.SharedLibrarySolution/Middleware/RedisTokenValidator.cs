using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace GoGetOffer.SharedLibrarySolution.Middleware
{
    public class RedisTokenValidator
    {
        private readonly IDatabase _redis;
        private readonly IConfiguration _config;
        private readonly RequestDelegate _next;
        private readonly IAesEncryptionHelperService _aesEncryptionHelperService;

        public RedisTokenValidator(
            RequestDelegate next,
            IConfiguration config,
            IConnectionMultiplexer redis,
            IAesEncryptionHelperService aesEncryptionHelperService)
        {
            _redis = redis.GetDatabase();
            _config = config;
            _next = next;
            _aesEncryptionHelperService = aesEncryptionHelperService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            var path = context.Request.Path.Value?.ToLower();
            if (path != null && (
                path.Contains("/api/auth/register") ||
                path.Contains("/api/auth/login") ||
                path.Contains("/api/auth/refresh-token") ||
                path.Contains("/api/auth/profile/password/forget") ||
                path.Contains("/api/auth/profile/password/reset") ||
                path.Contains("/api/health")))
            {
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var response = Response<string>.Failure("Authorization header is missing or invalid.");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrWhiteSpace(token) || token == "-")
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var response = Response<string>.Failure("Access token is missing or malformed.");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            var key = Encoding.UTF8.GetBytes(_config["Authentication:Key"]!);
            if (key is null)
                await Unauthorized(context, "key is invalid.");

            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal? principal = null;

            try
            {
                principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["Authentication:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Authentication:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out _);
            }
            catch (SecurityTokenExpiredException)
            {
                // التوكن منتهي
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var response = Response<string>.Failure("Token has expired.");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                // التوقيع مش صح
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var response = Response<string>.Failure("Invalid token signature.");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }
            catch (SecurityTokenException ex)
            {
                // أي مشاكل في الـ Token
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var response = Response<string>.Failure($"Token validation failed: {ex.Message}");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }
            catch (Exception ex)
            {
                // أي Exceptions عامة
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var response = Response<string>.Failure($"Unexpected error during token validation: {ex.Message}");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            if (principal is null)
                await Unauthorized(context, "Token is invalid (no principal).");

            var userId = principal!.FindFirst("user")?.Value;
            if (userId is null)
            {
                await Unauthorized(context, "Token is invalid (no user claim).");
                return;
            }

            var redisKey = $"token:user:{userId}";
            var storedToken = await _redis.StringGetAsync(redisKey);

            if (storedToken.IsNullOrEmpty)
            {
                await Unauthorized(context, "You Are Not Login.");
                return;
            }

            var decryptString = _aesEncryptionHelperService.DecryptString(storedToken!);
            if (decryptString != token)
            {
                await Unauthorized(context, "Access token is invalid or expired.");
                return;
            }

            context.User = principal;
            await _next(context);
        }

        private static async Task Unauthorized(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var response = Response<string>.Failure(message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            await context.Response.CompleteAsync();
        }
    }
}