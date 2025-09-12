using AuthenticationApi.Application.DependencyInjection;
using AuthenticationApi.Infrastructure.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();


builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Middleware
app.UseInfrastructurePolicies();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
