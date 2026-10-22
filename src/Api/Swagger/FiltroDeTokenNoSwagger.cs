using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Swagger;

public class FiltroDeTokenNoSwagger : IOperationFilter
{
    public void Apply(OpenApiOperation operacao, OperationFilterContext contexto)
    {
        var aberta = contexto.ApiDescription.ActionDescriptor.EndpointMetadata
            .OfType<IAllowAnonymous>()
            .Any();

        if (aberta)
            return;

        operacao.Security ??= [];
        operacao.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", contexto.Document)] = []
        });
    }
}
