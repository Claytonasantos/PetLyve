using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetLyve.API.Configuration.Swagger;

/// <summary>
/// Cria um documento do Swagger para cada versão descoberta pelo ApiExplorer.
/// </summary>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private const string BaseDescription =
        "API REST para gerenciamento de pets, donos e serviços.";

    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfo(description));
        }
    }

    private static OpenApiInfo CreateInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "PetLyve API",
            Version = description.ApiVersion.ToString(),
            Description = BaseDescription
        };

        if (description.IsDeprecated)
        {
            info.Title += " (DEPRECIADA)";
            info.Description +=
                " ATENÇÃO: esta versão está DEPRECIADA. Ela continua no ar " +
                "para não quebrar clientes existentes (GET /api/donos devolve " +
                "a lista completa, sem paginação), mas será removida no futuro. " +
                "Migre para a v2, que devolve um envelope paginado.";
        }

        return info;
    }
}
