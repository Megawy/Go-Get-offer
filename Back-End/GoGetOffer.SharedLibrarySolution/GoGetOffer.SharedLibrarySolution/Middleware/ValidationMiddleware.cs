using FluentValidation;
using FluentValidation.Results;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace GoGetOffer.SharedLibrarySolution.Middleware
{
    public class ValidationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            if (context.Request.Method == HttpMethods.Post ||
                context.Request.Method == HttpMethods.Put ||
                context.Request.Method == HttpMethods.Patch)
            {
                if (context.Request.ContentType != null &&
                    context.Request.ContentType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase))
                {
                    await _next(context);
                    return;
                }

                context.Request.EnableBuffering();

                var endpoint = context.GetEndpoint();
                var actionDescriptor = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();
                if (actionDescriptor != null)
                {
                    foreach (var parameter in actionDescriptor.Parameters)
                    {
                        var parameterType = parameter.ParameterType;
                        context.Request.Body.Position = 0;
                        var dto = await JsonSerializer.DeserializeAsync(
                            context.Request.Body,
                            parameterType,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (dto != null)
                        {
                            var validatorType = typeof(IValidator<>).MakeGenericType(parameterType);
                            var validator = serviceProvider.GetService(validatorType) as IValidator;
                            if (validator != null)
                            {
                                ValidationResult validationResult =
                                    await validator.ValidateAsync(new ValidationContext<object>(dto));

                                if (!validationResult.IsValid)
                                {
                                    var errors = validationResult.Errors
                                        .GroupBy(e => e.PropertyName)
                                        .ToDictionary(
                                            g => g.Key,
                                            g => g.Select(e => e.ErrorMessage).ToArray()
                                        );

                                    var response = Response<Dictionary<string, string[]>>.Failure(errors, "Validation Failed");

                                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                    context.Response.ContentType = "application/json";
                                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                                    return;
                                }
                            }
                        }
                    }
                }

                context.Request.Body.Position = 0;
            }

            await _next(context);
        }
    }
}
