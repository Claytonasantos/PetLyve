namespace PetLyve.API.Configuration.RateLimiting;

/// <summary>
/// Seção "RateLimiting" do appsettings.json.
/// </summary>
public class RateLimitingSettings
{
    public const string SectionName = "RateLimiting";

    public FixedWindowSettings Escrita { get; set; } = new()
    {
        PermitLimit = 10,
        WindowSeconds = 60
    };

    public FixedWindowSettings Leitura { get; set; } = new()
    {
        PermitLimit = 60,
        WindowSeconds = 60
    };
}

public class FixedWindowSettings
{
    public int PermitLimit { get; set; }

    public int WindowSeconds { get; set; }

    public TimeSpan Window => TimeSpan.FromSeconds(WindowSeconds);
}
