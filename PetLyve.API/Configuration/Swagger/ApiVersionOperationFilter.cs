using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetLyve.API.Configuration.Swagger;

/// <summary>
/// Marca no Swagger as operações de versões deprecadas e documenta os
/// parâmetros de versão (query e header) gerados pelo ApiExplorer.
/// </summary>
public class ApiVersionOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated;

        if (operation.Parameters is null)
        {
            return;
        }

        var versionParameters = operation.Parameters
            .OfType<OpenApiParameter>()
            .Where(parameter => parameter.Name is
                ApiVersioningConfiguration.QueryStringParameter or
                ApiVersioningConfiguration.HeaderName);

        foreach (var parameter in versionParameters)
        {
            parameter.Description ??=
                "Versão da API. Opcional: sem versão, a requisição cai na 2.0.";
        }
    }
}
