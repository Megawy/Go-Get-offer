using GoGetOffer.ApiGateway.Solution.Middelware;
using GoGetOffer.SharedLibrarySolution.DependencyInjection;
using GoGetOffer.SharedLibrarySolution.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text.Json;
using System.Text.Json.Nodes;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var files = new[]
{
    "ocelot.global.json",
    // AuthenticationApi
    // Authentication
    "AuthenticationApi/Authentication/ocelot.admin.json",
    "AuthenticationApi/Authentication/ocelot.auth.json",
    "AuthenticationApi/Authentication/ocelot.email.json",
    "AuthenticationApi/Authentication/ocelot.password.json",
    "AuthenticationApi/Authentication/ocelot.user.json",
    
    // Supplier
    "AuthenticationApi/Supplier/ocelot.admin.json",
    "AuthenticationApi/Supplier/ocelot.branch.json",
    "AuthenticationApi/Supplier/ocelot.supplier.json",
    "AuthenticationApi/Supplier/ocelot.update.json",

    // ProductApi
    // Category
    "ProductApi/Category/ocelot.category.json",
};

var mergedConfig = new JsonObject();
var allRoutes = new JsonArray();

foreach (var file in files)
{
    var jsonText = File.ReadAllText(file);
    var jsonDoc = JsonNode.Parse(jsonText)!.AsObject();

    if (jsonDoc["Routes"] is JsonArray routes)
    {
        foreach (var route in routes)
        {
            allRoutes.Add(route!.DeepClone());
        }
    }


    if (jsonDoc["GlobalConfiguration"] is JsonObject globalConfig)
    {
        mergedConfig["GlobalConfiguration"] = globalConfig.DeepClone();
    }
}

mergedConfig["Routes"] = allRoutes;

var memoryStream = new MemoryStream();
using (var writer = new Utf8JsonWriter(memoryStream))
{
    mergedConfig.WriteTo(writer);
}
memoryStream.Position = 0;

builder.Configuration.AddJsonStream(memoryStream);


builder.Services.AddOcelot().AddCacheManager(x => x.WithDictionaryHandle());

JWTAuthenticationScheme.AddJWTAuthenticationScheme(builder.Services, builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "https://f32315196909.ngrok-free.app")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");


app.UseMiddleware<AttachSignatrueToRequset>();
app.UseMiddleware<GlobalException>();

app.UseOcelot().Wait();

app.Run();
