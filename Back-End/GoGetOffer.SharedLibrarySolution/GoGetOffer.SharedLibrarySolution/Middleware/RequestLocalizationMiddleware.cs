using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace GoGetOffer.SharedLibrarySolution.Middleware
{
    public class RequestLocalizationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        public async Task InvokeAsync(HttpContext context)
        {
            var acceptLang = context.Request.Headers["Accept-Language"].ToString();
            var culture = "en-US"; // default


            if (!string.IsNullOrEmpty(acceptLang))
            {
                var lang = acceptLang.Split(',')[0];

                culture = lang;
                var cultureInfo = new CultureInfo(culture);
                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.CurrentUICulture = cultureInfo;
            }

            await _next(context);
        }
    }
}
