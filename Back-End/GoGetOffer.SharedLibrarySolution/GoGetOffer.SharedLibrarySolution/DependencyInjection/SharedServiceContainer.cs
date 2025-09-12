using FluentValidation;
using FluentValidation.AspNetCore;
using GoGetOffer.SharedLibrarySolution.Interface;
using GoGetOffer.SharedLibrarySolution.Interface.Email;
using GoGetOffer.SharedLibrarySolution.Interface.Helper;
using GoGetOffer.SharedLibrarySolution.Interface.JWT;
using GoGetOffer.SharedLibrarySolution.Interface.Redis;
using GoGetOffer.SharedLibrarySolution.Logs;
using GoGetOffer.SharedLibrarySolution.Middleware;
using GoGetOffer.SharedLibrarySolution.Responses;
using GoGetOffer.SharedLibrarySolution.Service;
using GoGetOffer.SharedLibrarySolution.Service.Email;
using GoGetOffer.SharedLibrarySolution.Service.Helper;
using GoGetOffer.SharedLibrarySolution.Service.JWT;
using GoGetOffer.SharedLibrarySolution.Service.Redis;
using GoGetOffer.SharedLibrarySolution.Validator;
using GoGetOffer.SharedLibrarySolution.Validator.Auth.User.ConfirmEmail;
using GoGetOffer.SharedLibrarySolution.Validator.Auth.User.Login;
using GoGetOffer.SharedLibrarySolution.Validator.Auth.User.Register;
using GoGetOffer.SharedLibrarySolution.Validator.Auth.User.UpdateUser;
using GoGetOffer.SharedLibrarySolution.Validator.Auth.User.UserPassword;
using GoGetOffer.SharedLibrarySolution.Validator.Product.Category;
using GoGetOffer.SharedLibrarySolution.Validator.Suppliers.Branch;
using GoGetOffer.SharedLibrarySolution.Validator.Suppliers.JoinRequest;
using GoGetOffer.SharedLibrarySolution.Validator.Suppliers.Query;
using GoGetOffer.SharedLibrarySolution.Validator.Suppliers.Register;
using GoGetOffer.SharedLibrarySolution.Validator.Suppliers.Update;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using Polly;
using Polly.Retry;
using Serilog;
using StackExchange.Redis;
using static GoGetOffer.SharedLibrarySolution.Service.Helper.AesEncryptionHelperService;

namespace GoGetOffer.SharedLibrarySolution.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>
            (this IServiceCollection services, IConfiguration config, string filename) where TContext : DbContext
        {


            // Add Generic Database context
            string connectionString = config.GetConnectionString("Connection")!;
            services.AddDbContext<TContext>(option => option.UseSqlServer(
                connectionString, SqlServerOption =>
                SqlServerOption.EnableRetryOnFailure()));

            //configure Cloudinary
            services.AddGenericCloudinary(config);

            // configure Encryption Key
            services.Configure<EncryptionSettings>(config.GetSection("Encryption"));

            //configure hangfire
            services.AddHangfire(hangfireConfig =>
                hangfireConfig.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                              .UseSimpleAssemblyNameTypeSerializer()
                              .UseRecommendedSerializerSettings()
                              .UseSqlServerStorage(
                                  connectionString,
                                  new SqlServerStorageOptions
                                  {
                                      CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                      SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                      QueuePollInterval = TimeSpan.Zero,
                                      UseRecommendedIsolationLevel = true,
                                      DisableGlobalLocks = true
                                  }
                              ));
            services.AddHangfireServer();

            // configure Serilog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "GoGetOffer.API")
                .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production")
                .WriteTo.Debug()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
                )
                .WriteTo.File(
                path: $"Logs/{filename}-.txt",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();


            // Create Retry Strategy 
            var retryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder()
                .Handle<TaskCanceledException>()
                .Handle<HttpRequestException>()
                .Handle<TimeoutException>()
                .Handle<OperationCanceledException>(),

                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                MaxRetryAttempts = 5,
                Delay = TimeSpan.FromSeconds(5),
                OnRetry = args =>
                {
                    string message = $"OnRetry, Attempt: {args.AttemptNumber} Outcome: {args.Outcome.Exception?.Message}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    LogException.LogToFile(message);
                    return ValueTask.CompletedTask;
                }
            };

            // Use Retry strategy
            services.AddResiliencePipeline("my-retry-pipeline", builder =>
            {
                builder.AddRetry(retryStrategy);
            });

            // Add JWT authentication Scheme
            services.AddJWTAuthenticationScheme(config);

            // Create Dependency Injection
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IGenericCloudinary, GenericCloudinaryService>();
            services.AddScoped<IGenericRedisService, GenericRedisService>();
            services.AddScoped<IValidateUploadAndEncrypt, ValidateUploadAndEncrypt>();
            services.AddTransient<IAesEncryptionHelperService, AesEncryptionHelperService>();
            services.AddScoped<IHelperMethodService, HelperMethodService>();
            services.AddScoped<IJwtService, JwtService>();

            // Run Services AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(config["Redis:ConnectionString"]!);
                configuration.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(configuration);
            });

            // ✅ Register common dependencies
            services.AddHttpContextAccessor();

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                "application/json"
                });
            });

            // ✅ Configure EPPlus License (EPPlus 8+)
            ExcelPackage.License.SetNonCommercialPersonal("Megawy");

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Fastest; // حدد namespace كامل
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Fastest; // حدد namespace كامل
            });

            // ✅ Register FluentValidation (Auto-validation + Client side)
            services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();


            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

            // ✅ Register Validators
            var assemblies = new[]
            {
                // ID
                typeof(IDValidator).Assembly,
                
                // Authentication API
                // Authentication Auth
                typeof(RegisterValidator).Assembly,
                typeof(LoginValidator).Assembly,
                typeof(RequestUserUpdateValidator).Assembly,
                typeof(ApproveUserUpdateValidator).Assembly,
                
                // Authentication Password
                typeof(ResetPasswordValidator).Assembly,
                typeof(ForgotPasswordValidator).Assembly,
                typeof(ChangePasswordValidator).Assembly,

                // Authentication Email
                typeof(ConfirmEmailOtpValidator).Assembly,
                typeof(ChangeRoleValidator).Assembly,

                // Supplier
                // Supplier Branch Command 
                typeof(CreateBranchValidator).Assembly,
                typeof(UpdateBranchValidator).Assembly,

                // Supplier Profile Command 
                typeof(CodeValidator).Assembly,
                typeof(RegisterSupplierValidator).Assembly,
                typeof(AddSomeDataSupplierValidator).Assembly,

                // Supplier Join Request Command
                typeof(ReplyRequestJoinValidator).Assembly,

                // Supplier Update Command
                typeof(ApproveSupplierUpdateValidator).Assembly,
                typeof(CreateRequestSupplierUpdateProfileValidator).Assembly,
                // End Authentication API

                // ProductApi
                // Category
                typeof(CreateCategoryValidator).Assembly,
                typeof(CategoryTranslationValidator).Assembly,
                typeof(EditCategoryValidator).Assembly,

                // End ProductApi
                };

            foreach (var assembly in assemblies)
            {
                services.AddValidatorsFromAssembly(assembly);
            }

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    var response = Response<Dictionary<string, string[]>>.Failure(errors, "Validation Failed");

                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }

        public static IApplicationBuilder UseSharedPolicies<TContext>(this IApplicationBuilder app)
            where TContext : DbContext
        {
            // 🔹 Exception handling في الأول عشان يمسك أي Error
            app.UseMiddleware<GlobalException>();

            // 🔹 Logging & Request Id
            app.UseMiddleware<SerilogRequestIdMiddleware>();
            app.UseMiddleware<LoggingAndTrackingMiddleware>();

            // 🔹 Localization
            app.UseMiddleware<RequestLocalizationMiddleware>();

            // 🔹 Rate limiting & Gateway
            app.UseMiddleware<RateLimitingMiddleware>();
            app.UseMiddleware<ListenToOnlyApiGateway>();

            // 🔹 Auth (JWT + Redis)
            app.UseMiddleware<RedisTokenValidator>();

            // 🔹 Validation (DTOs)
            app.UseMiddleware<ValidationMiddleware>();

            // 🔹 DB Transactions
            app.UseMiddleware<TransactionMiddleware<TContext>>();

            // 🔹 Response caching (اختياري)
            //app.UseMiddleware<RedisResponseCachingMiddleware>();

            // 🔹 Response compression (اختياري)
            //app.UseResponseCompression();

            return app;
        }
    }
}
