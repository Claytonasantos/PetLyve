namespace PetLyve.API.Configuration.RateLimiting;

/// <summary>
/// Nomes das políticas usadas em [EnableRateLimiting].
/// </summary>
public static class RateLimitPolicies
{
    /// <summary>Endpoints de escrita (POST/PUT). Padrão: 10 requisições/60 s por IP.</summary>
    public const string Escrita = "escrita";

    /// <summary>Listagem paginada v2, mais branda. Padrão: 60 requisições/60 s por IP.</summary>
    public const string Leitura = "leitura";
}
