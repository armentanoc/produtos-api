namespace Api.Inicializacao;

public static class BancoSqlite
{
    public static string DefinirConexao(WebApplicationBuilder builder)
    {
        var configurada = builder.Configuration.GetConnectionString("Catalogo");
        var usaPadrao = string.IsNullOrWhiteSpace(configurada)
            || configurada.Contains("dados/catalogo.db", StringComparison.OrdinalIgnoreCase);

        string conexao;
        if (usaPadrao && Environment.GetEnvironmentVariable("VERCEL") == "1")
            conexao = "Data Source=/tmp/catalogo.db";
        else if (usaPadrao)
        {
            var pasta = Path.Combine(builder.Environment.ContentRootPath, "dados");
            Directory.CreateDirectory(pasta);
            conexao = $"Data Source={Path.Combine(pasta, "catalogo.db")}";
        }
        else
            conexao = configurada!;

        GarantirPasta(conexao);
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:Catalogo"] = conexao
        });

        return conexao;
    }

    private static void GarantirPasta(string conexao)
    {
        const string marcador = "Data Source=";
        var indice = conexao.IndexOf(marcador, StringComparison.OrdinalIgnoreCase);
        if (indice < 0)
            return;

        var caminho = conexao[(indice + marcador.Length)..].Split(';')[0].Trim().Trim('"');
        var pasta = Path.GetDirectoryName(caminho);
        if (!string.IsNullOrEmpty(pasta))
            Directory.CreateDirectory(pasta);
    }
}
