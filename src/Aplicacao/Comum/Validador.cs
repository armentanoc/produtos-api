namespace Aplicacao;

public static class Validador
{
    public static string Texto(string? valor, string mensagemDeVazio, int maximo)
    {
        var texto = valor?.Trim() ?? "";
        if (texto.Length == 0)
            throw new ExcecaoDeRequisicaoInvalida(mensagemDeVazio);
        if (texto.Length > maximo)
            throw new ExcecaoDeRequisicaoInvalida($"Use no máximo {maximo} caracteres.");
        return texto;
    }

    public static IReadOnlyList<string> Urls(IEnumerable<string>? urls, bool obrigatorio)
    {
        var lista = (urls ?? []).Select(Url).ToList();
        if (obrigatorio && lista.Count == 0)
            throw new ExcecaoDeRequisicaoInvalida("Informe ao menos uma URL de imagem.");
        if (lista.Count > 20)
            throw new ExcecaoDeRequisicaoInvalida("Cadastre no máximo 20 imagens por vez.");
        return lista;
    }

    public static string Url(string? valor)
    {
        var texto = valor?.Trim() ?? "";
        var enderecoValido = Uri.TryCreate(texto, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

        if (!enderecoValido)
            throw new ExcecaoDeRequisicaoInvalida("Cada imagem precisa de uma URL que comece com http:// ou https://.");

        if (texto.Length > 2000)
            throw new ExcecaoDeRequisicaoInvalida("A URL da imagem pode ter no máximo 2000 caracteres.");

        return texto;
    }
}
