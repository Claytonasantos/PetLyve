using System.Collections.Concurrent;
using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace PetLyve.API.Configuration.RateLimiting;

/// <summary>
/// Política fixed window particionada por IP do cliente. Cada IP ganha o seu
/// próprio <see cref="FixedWindowRateLimiter"/>. Além de limitar, a política
/// escreve X-RateLimit-Limit / Remaining / Reset em todas as respostas.
/// </summary>
public sealed class FixedWindowPerIpPolicy : IRateLimiterPolicy<string>
{
    private readonly FixedWindowSettings _settings;

    // Limitador ativo de cada IP, usado só para ler estatísticas da janela.
    // O ciclo de vida continua com o middleware: quando ele recria a partição
    // (após descartar um limitador ocioso), a entrada é substituída.
    private readonly ConcurrentDictionary<string, WindowState> _windows = new();

    public FixedWindowPerIpPolicy(FixedWindowSettings settings)
    {
        if (settings.PermitLimit < 1 || settings.WindowSeconds < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(settings),
                "PermitLimit e WindowSeconds devem ser maiores que zero.");
        }

        _settings = settings;
    }

    // null: usa o OnRejected global configurado em AddRateLimiter.
    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => null;

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        var partitionKey =
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "desconhecido";

        httpContext.Features.Set<IRateLimitWindowFeature>(
            new RateLimitWindowFeature(this, partitionKey));

        httpContext.Response.OnStarting(() =>
        {
            WriteRateLimitHeaders(httpContext.Response, partitionKey);
            return Task.CompletedTask;
        });

        return RateLimitPartition.Get(partitionKey, CreateLimiter);
    }

    private RateLimiter CreateLimiter(string partitionKey)
    {
        var limiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
        {
            PermitLimit = _settings.PermitLimit,
            Window = _settings.Window,
            QueueLimit = 0,
            AutoReplenishment = true
        });

        _windows[partitionKey] = new WindowState(limiter, DateTimeOffset.UtcNow);

        return limiter;
    }

    private void WriteRateLimitHeaders(HttpResponse response, string partitionKey)
    {
        var remaining = GetRemaining(partitionKey);
        var reset = GetSecondsUntilReset(partitionKey);

        response.Headers["X-RateLimit-Limit"] =
            _settings.PermitLimit.ToString(CultureInfo.InvariantCulture);
        response.Headers["X-RateLimit-Remaining"] =
            remaining.ToString(CultureInfo.InvariantCulture);
        response.Headers["X-RateLimit-Reset"] =
            reset.ToString(CultureInfo.InvariantCulture);
    }

    private long GetRemaining(string partitionKey)
    {
        if (!_windows.TryGetValue(partitionKey, out var state))
        {
            return _settings.PermitLimit;
        }

        return state.Limiter.GetStatistics()?.CurrentAvailablePermits
            ?? _settings.PermitLimit;
    }

    /// <summary>
    /// Segundos até a próxima janela. O FixedWindowRateLimiter reabastece em
    /// intervalos fixos contados a partir da sua criação.
    /// </summary>
    public int GetSecondsUntilReset(string partitionKey)
    {
        if (!_windows.TryGetValue(partitionKey, out var state))
        {
            return _settings.WindowSeconds;
        }

        var window = _settings.Window;
        var elapsed = DateTimeOffset.UtcNow - state.CreatedAt;
        var intoWindow = TimeSpan.FromTicks(elapsed.Ticks % window.Ticks);
        var remaining = window - intoWindow;

        return Math.Max(1, (int)Math.Ceiling(remaining.TotalSeconds));
    }

    private sealed record WindowState(
        FixedWindowRateLimiter Limiter,
        DateTimeOffset CreatedAt);

    private sealed class RateLimitWindowFeature(
        FixedWindowPerIpPolicy policy,
        string partitionKey) : IRateLimitWindowFeature
    {
        public int GetSecondsUntilReset() =>
            policy.GetSecondsUntilReset(partitionKey);
    }
}

/// <summary>
/// Exposto no HttpContext para o OnRejected calcular o Retry-After exato.
/// </summary>
public interface IRateLimitWindowFeature
{
    int GetSecondsUntilReset();
}
