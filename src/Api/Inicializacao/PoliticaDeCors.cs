namespace Api.Inicializacao;

public static class PoliticaDeCors
{
    public const string Nome = "Catalogo";

    public static void Adicionar(IServiceCollection servicos, IConfiguration configuracao)
    {
        var origens = configuracao.GetSection("Cors:OrigensPermitidas").Get<string[]>() ?? [];
        var permitirVercel = configuracao.GetValue("Cors:PermitirProjetosVercel", false);
        var lista = origens
            .Select(Normalizar)
            .Where(origem => origem.Length > 0)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        servicos.AddCors(opcoes =>
        {
            opcoes.AddPolicy(Nome, politica =>
            {
                politica
                    .SetIsOriginAllowed(origem => Permitida(origem, lista, permitirVercel))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    private static bool Permitida(string origem, HashSet<string> lista, bool permitirVercel)
    {
        if (lista.Contains(Normalizar(origem)))
            return true;

        if (!permitirVercel || !Uri.TryCreate(origem, UriKind.Absolute, out var uri))
            return false;

        return uri.Scheme == Uri.UriSchemeHttps
            && uri.Host.EndsWith(".vercel.app", StringComparison.OrdinalIgnoreCase);
    }

    private static string Normalizar(string origem) => origem.Trim().TrimEnd('/');
}
