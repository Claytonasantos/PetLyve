using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace PetLyve.API.Configuration.RateLimiting;

public static class RateLimitingConfiguration
{
    /// <summary>
    /// Registra o rate limiter nativo com as políticas nomeadas
    /// <see cref="RateLimitPolicies.Escrita"/> e <see cref="RateLimitPolicies.Leitura"/>.
    /// Não há limitador global: só os endpoints marcados com
    /// [EnableRateLimiting] são limitados (o /health fica de fora).
    /// </summary>
    public static IServiceCollection AddPetLyveRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(RateLimitingSettings.SectionName)
            .Get<RateLimitingSettings>() ?? new RateLimitingSettings();

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy(
                RateLimitPolicies.Escrita,
                new FixedWindowPerIpPolicy(settings.Escrita));

            options.AddPolicy(
                RateLimitPolicies.Leitura,
                new FixedWindowPerIpPolicy(settings.Leitura));

            options.OnRejected = WriteRejectionAsync;
        });

        return services;
    }

    private static async ValueTask WriteRejectionAsync(
        OnRejectedContext context,
        CancellationToken cancellationToken)
    {
        var httpContext = context.HttpContext;
        var traceId = httpContext.TraceIdentifier;

        var retryAfterSeconds = GetRetryAfterSeconds(context);

        httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        httpContext.Response.Headers.RetryAfter =
            retryAfterSeconds.ToString(CultureInfo.InvariantCulture);

        var logger = httpContext.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger(typeof(RateLimitingConfiguration));

        logger.LogWarning(
            "Limite de requisições excedido. Método={Method}, Path={Path}, IP={RemoteIp}, RetryAfter={RetryAfter}s, TraceId={TraceId}",
            httpContext.Request.Method,
            httpContext.Request.Path,
            httpContext.Connection.RemoteIpAddress,
            retryAfterSeconds,
            traceId);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status429TooManyRequests,
            Title = "Limite de requisições excedido",
            Detail =
                $"Muitas requisições para este endpoint. Tente novamente em {retryAfterSeconds} segundo(s).",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["retryAfterSeconds"] = retryAfterSeconds;
        problemDetails.Extensions["traceId"] = traceId;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            options: null,
            contentType: "application/problem+json",
            cancellationToken: cancellationToken);
    }

    private static int GetRetryAfterSeconds(OnRejectedContext context)
    {
        var windowFeature = context.HttpContext.Features.Get<IRateLimitWindowFeature>();

        if (windowFeature is not null)
        {
            return windowFeature.GetSecondsUntilReset();
        }

        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            return Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
        }

        return 60;
    }
}
