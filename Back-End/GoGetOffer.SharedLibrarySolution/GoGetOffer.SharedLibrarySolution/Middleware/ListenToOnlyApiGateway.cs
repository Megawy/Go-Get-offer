using Microsoft.AspNetCore.Http;
using System.Text.Json;
using GoGetOffer.SharedLibrarySolution.Responses;

namespace GoGetOffer.SharedLibrarySolution.Middleware
{
    public class ListenToOnlyApiGateway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var signedHeader = context.Request.Headers["Api-Gateway"];

            if (signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.ContentType = "application/json";
                var response = Response<string>.Failure("Sorry, service is unavailable");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            await next(context);
        }
    }
}
