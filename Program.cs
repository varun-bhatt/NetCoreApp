using System.Net;
using MediatR;
using Peddle.Foundation.Common.Filters;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NetCoreApp.Extensions;
using NetCoreApp.Infrastructure.Persistence;
using NetCoreApplication.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(config =>
{
    config.Filters.Add(new ValidateModelFilterAttribute());
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
builder.Services.AddWebApiServicesExtension(builder.Configuration);
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidators(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Configure Kestrel to listen on a specific address and port
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 5000); // Replace with your desired address and port
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "";
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseErrorHandlingMiddleware();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    // Simple health check endpoint checks that endpoint is accessible or not
    endpoints.MapHealthChecks("/api/v1/health/liveness", new HealthCheckOptions
    {
        Predicate = check => false, // for only basic check
        ResponseWriter = null, // To send blank response body
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status500InternalServerError
        }
    });

    // Execute only checks with tags Readiness
    endpoints.MapHealthChecks("/api/v1/health/readiness", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("Readiness"),
        ResponseWriter = null,
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status500InternalServerError
        }
    });
});

app.Run();