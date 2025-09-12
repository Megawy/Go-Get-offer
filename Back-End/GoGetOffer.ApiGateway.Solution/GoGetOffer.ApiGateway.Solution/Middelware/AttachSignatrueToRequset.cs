namespace GoGetOffer.ApiGateway.Solution.Middelware
{
    public class AttachSignatrueToRequset(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers["Api-Gateway"] = "Signed";
            await next(context);
        }
    }
}
