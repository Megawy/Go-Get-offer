using GoGetOffer.SharedLibrarySolution.Logs;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Net;
using System.Text.Json;

namespace GoGetOffer.SharedLibrarySolution.Middleware;

public class GlobalException(RequestDelegate next, ILogger<GlobalException> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalException> _logger = logger;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;

        try
        {
            await _next(context);

            // التعامل مع StatusCodes غير Success
            await HandleNonSuccessStatusCodes(context);
        }
        catch (Exception ex)
        {
            // Logging
            LogException.LogExceptions(ex);
            using (LogContext.PushProperty("RequestId", requestId))
            {
                _logger.LogError(ex, "❌ Unhandled exception occurred. RequestId: {RequestId}", requestId);
            }
            context.Response.ContentType = "application/json";


            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";

            if (ex is TaskCanceledException || ex is TimeoutException)
            {
                statusCode = StatusCodes.Status408RequestTimeout;
                message = "Request timeout....try again";
            }

            await WriteResponseAsync(context, message, statusCode, new { exception = ex.Message, traceId = requestId });
        }
    }

    private static async Task HandleNonSuccessStatusCodes(HttpContext context)
    {
        if (context.Response.HasStarted) return;

        string message = null!;
        int? statusCode = null;

        switch (context.Response.StatusCode)
        {
            case StatusCodes.Status400BadRequest:
                message = "Invalid request data.";
                statusCode = StatusCodes.Status400BadRequest;
                break;
            case StatusCodes.Status401Unauthorized:
                message = "You are not authorized to access.";
                statusCode = StatusCodes.Status401Unauthorized;
                break;
            case StatusCodes.Status403Forbidden:
                message = "You are not allowed to access.";
                statusCode = StatusCodes.Status403Forbidden;
                break;
            case StatusCodes.Status404NotFound:
                message = "The requested resource was not found.";
                statusCode = StatusCodes.Status404NotFound;
                break;
            case StatusCodes.Status429TooManyRequests:
                message = "Too many requests made.";
                statusCode = StatusCodes.Status429TooManyRequests;
                break;
            case StatusCodes.Status502BadGateway:
                message = "The server, working as a gateway, got an invalid response.";
                statusCode = StatusCodes.Status502BadGateway;
                break;
            case StatusCodes.Status500InternalServerError:
                message = "The server has encountered a situation it does not know how to handle.";
                statusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        if (message != null && statusCode.HasValue)
        {
            await WriteResponseAsync(context, message, statusCode.Value);
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, string message, int statusCode, object? errors = null)
    {
        if (context.Response.HasStarted) return;

        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = Response<string>.Failure(errors ?? message);
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
    }
}
