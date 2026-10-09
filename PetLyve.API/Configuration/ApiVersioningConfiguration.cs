using Asp.Versioning;

namespace PetLyve.API.Configuration;

public static class ApiVersioningConfiguration
{
    public const string QueryStringParameter = "api-version";

    public const string HeaderName = "X-Api-Version";

    /// <summary>
    /// Registra o versionamento da API. Sem versão na requisição, cai na 2.0.
    /// A versão pode ser informada por query string (api-version), header
    /// (X-Api-Version) ou segmento de URL (/api/v{versao}/...).
    /// </summary>
    public static IServiceCollection AddPetLyveApiVersioning(
        this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(2, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader(QueryStringParameter),
                    new HeaderApiVersionReader(HeaderName),
                    new UrlSegmentApiVersionReader());
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                // Grupos "v1" e "v2" no Swagger.
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}
