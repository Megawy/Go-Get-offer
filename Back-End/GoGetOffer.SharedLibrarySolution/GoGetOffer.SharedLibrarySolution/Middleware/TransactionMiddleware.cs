using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GoGetOffer.SharedLibrarySolution.Middleware
{
    public sealed class TransactionMiddleware<TContext> where TContext : DbContext
    {
        private readonly RequestDelegate _next;
        public TransactionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, TContext db)
        {
            if (HttpMethods.IsGet(context.Request.Method))
            {
                await _next(context);
                return;
            }

            var strategy = db.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync();
                await _next(context);

                if (context.Response.StatusCode < 400)
                    await tx.CommitAsync();
                else
                    await tx.RollbackAsync();
            });
        }
    }
}