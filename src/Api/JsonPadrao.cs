namespace Api;

public static class JsonPadrao
{
    public static readonly JsonSerializerOptions Opcoes = Criar();

    public static void Aplicar(JsonSerializerOptions destino)
    {
        destino.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        destino.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    }

    private static JsonSerializerOptions Criar()
    {
        var opcoes = new JsonSerializerOptions();
        Aplicar(opcoes);
        return opcoes;
    }
}
