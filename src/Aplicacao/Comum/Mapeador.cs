namespace Aplicacao;

public static class Mapeador
{
    public static ProdutoResposta ParaResposta(Produto produto) => new()
    {
        Id = produto.Id,
        Nome = produto.Nome,
        Descricao = produto.Descricao,
        DataCadastro = produto.DataCadastro,
        DataAtualizacao = produto.DataAtualizacao,
        EstaAtivo = produto.EstaAtivo,
        Imagens = produto.Imagens.OrderBy(imagem => imagem.Id).Select(ParaResposta).ToList()
    };

    public static ImagemResposta ParaResposta(Imagem imagem) => new()
    {
        Id = imagem.Id,
        ProdutoId = imagem.ProdutoId,
        Url = imagem.Url,
        DataCadastro = imagem.DataCadastro
    };
}
