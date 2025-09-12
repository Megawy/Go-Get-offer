using AuthenticationApi.Application.Services.AuthenticationService;
using AuthenticationApi.Application.Services.AuthenticationService.ActionService;
using AuthenticationApi.Application.Services.AuthenticationService.AuthService;
using AuthenticationApi.Application.Services.AuthenticationService.EmailService;
using AuthenticationApi.Application.Services.AuthenticationService.PasswordService;
using AuthenticationApi.Application.Services.AuthenticationService.QueryService;
using AuthenticationApi.Application.Services.AuthenticationService.UserUpdateService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.ActionService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.AuthService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.EmailService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.PasswordService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.QueryService;
using AuthenticationApi.Application.Services.Interfaces.AuthenticationService.UserUpdateService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.BranceService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.JoinService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProFileService;
using AuthenticationApi.Application.Services.Interfaces.SupplierService.ProfileUpdateService;
using AuthenticationApi.Application.Services.SupplierService;
using AuthenticationApi.Application.Services.SupplierService.BranceService;
using AuthenticationApi.Application.Services.SupplierService.JoinService;
using AuthenticationApi.Application.Services.SupplierService.ProFileService;
using AuthenticationApi.Application.Services.SupplierService.ProfileUpdateService;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationApi.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            // Create Dependency Injection
            // Authentication
            // Helper Method User
            services.AddScoped<IUserMethodsService, UserMethodsService>();

            // User Action
            services.AddScoped<IUserActionService, UserActionService>();

            // User Authentication
            services.AddScoped<IUserAuthService, UserAuthService>();
            services.AddScoped<IRedisAuthService, RedisAuthService>();

            // User Email OTP
            services.AddScoped<IUserEmailService, UserEmailService>();
            services.AddScoped<IRedisEmailService, RedisEmailService>();

            // User Password  
            services.AddScoped<IUserPasswordService, UserPasswordService>();
            services.AddScoped<IRedisPasswordService, RedisPasswordService>();

            // User Query
            services.AddScoped<IUserQueryService, UserQueryService>();
            services.AddScoped<IRedisQueryService, RedisQueryService>();

            // User Update
            services.AddScoped<IUserUpdateQueryService, UserUpdateQueryService>();
            services.AddScoped<IUserUpdateCommandService, UserUpdateCommandService>();

            // Supplier 
            // Helper Method Supplier
            services.AddScoped<ISupplierMethodsService, SupplierMethodsService>();

            // Supplier Profile
            services.AddScoped<ISupplierCommandService, SupplierCommandService>();
            services.AddScoped<ISupplierQueryService, SupplierQueryService>();
            services.AddScoped<IRedisSupplierQueryService, RedisSupplierQueryService>();

            // Supplier Branch
            services.AddScoped<ISupplierBranceQueryService, SupplierBranceQueryService>();
            services.AddScoped<ISupplierBranchCommandService, SupplierBranchCommandService>();
            services.AddScoped<IRedisBranchService, RedisBranchService>();

            // Supplier Join
            services.AddScoped<ISupplierJoinQueryService, SupplierJoinQueryService>();
            services.AddScoped<ISupplierJoinCommandService, SupplierJoinCommandService>();
            services.AddScoped<IRedisJoinService, RedisJoinService>();

            // Supplier Update Request
            services.AddScoped<ISupplierUpdateCommandService, SupplierUpdateCommandService>();
            services.AddScoped<ISupplierUpdateQueryService, SupplierUpdateQueryService>();
            services.AddScoped<IRedisSupplierUpdateService, RedisSupplierUpdateService>();

            return services;
        }
    }
}