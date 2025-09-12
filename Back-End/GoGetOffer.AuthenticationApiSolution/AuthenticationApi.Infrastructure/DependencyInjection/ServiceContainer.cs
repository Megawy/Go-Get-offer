using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories.Authentication;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using AuthenticationApi.Infrastructure.Repositories.Supplier;
using GoGetOffer.SharedLibrarySolution.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            // ✅ Add shared services: DB, JWT, Serilog
            SharedServiceContainer.AddSharedServices<AuthenticationDbContext>(
                services,
                config,
                config["MySerilog:FileName"]!
            );


            // Create Dependency Injection
            // Authentication
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRequestUserUpdateRepository, RequestUserUpdateRepository>();

            // Supplier 
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IProfileSupplierUpdateRepository, ProfileSupplierUpdateRepository>();
            services.AddScoped<ISupplierJoinRequestRepository, SupplierJoinRequestRepository>();
            services.AddScoped<ISupplierBranchRepository, SupplierBranchRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicies(this IApplicationBuilder app)
        {
            // ✅ Apply shared middleware: exception handling, API Gateway restriction
            SharedServiceContainer.UseSharedPolicies<AuthenticationDbContext>(app);

            return app;
        }
    }
}
