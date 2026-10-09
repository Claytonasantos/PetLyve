using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using PetLyve.API.Configuration;
using PetLyve.API.Configuration.RateLimiting;
using PetLyve.API.Configuration.Swagger;
using PetLyve.API.Exceptions;
using PetLyve.Application;
using PetLyve.Application.Services;
using PetLyve.Infrastructure.Data;
using PetLyve.Infrastructure.Data.Repositories;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddPetLyveApiVersioning();

builder.Services.AddPetLyveRateLimiting(builder.Configuration);

// Um documento do Swagger por versão (v1 deprecada e v2).
builder.Services.AddTransient<
    IConfigureOptions<SwaggerGenOptions>,
    ConfigureSwaggerOptions>();

builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<ApiVersionOperationFilter>();

    var xmlFile =
        $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddDbContextCheck<ApplicationDbContext>("database");

builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));

builder.Services.AddScoped<DonoService>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Mais recente primeiro: a UI abre na v2 e permite trocar para a v1.
        foreach (var description in app.DescribeApiVersions().Reverse())
        {
            var name = description.IsDeprecated
                ? $"{description.GroupName.ToUpperInvariant()} (deprecada)"
                : description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                name);
        }
    });
}

// Depois do UseExceptionHandler e antes do MapControllers.
app.UseRateLimiter();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },

    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration,
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                duration = entry.Value.Duration,
                exception = app.Environment.IsDevelopment()
                    ? entry.Value.Exception?.Message
                    : null
            })
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
})
.DisableRateLimiting();

app.MapControllers();

app.Run();