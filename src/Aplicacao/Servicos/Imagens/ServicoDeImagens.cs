namespace Aplicacao.Servicos;

public class ServicoDeImagens : IServicoDeImagens
{
    private readonly IRepositorioDeImagens _imagens;
    private readonly IRepositorioDeProdutos _produtos;

    public ServicoDeImagens(IRepositorioDeImagens imagens, IRepositorioDeProdutos produtos)
    {
        _imagens = imagens;
        _produtos = produtos;
    }

    public async Task<IReadOnlyList<ImagemResposta>> ListarPorProduto(int produtoId)
    {
        if (!await _produtos.Existe(produtoId))
            throw new ExcecaoDeRecursoNaoEncontrado("Produto não encontrado.");

        var imagens = await _imagens.ListarPorProduto(produtoId);
        return imagens.Select(Mapeador.ParaResposta).ToList();
    }

    public async Task<ImagemResposta> Buscar(int id)
    {
        var imagem = await _imagens.BuscarPorId(id)
            ?? throw new ExcecaoDeRecursoNaoEncontrado("Imagem não encontrada.");

        return Mapeador.ParaResposta(imagem);
    }

    public async Task<IReadOnlyList<ImagemResposta>> Cadastrar(CadastrarImagens pedido)
    {
        if (!await _produtos.Existe(pedido.ProdutoId))
            throw new ExcecaoDeRecursoNaoEncontrado("Produto não encontrado.");

        var imagens = Validador.Urls(pedido.Urls, obrigatorio: true)
            .Select(url => new Imagem { ProdutoId = pedido.ProdutoId, Url = url })
            .ToList();

        await _imagens.CadastrarVarias(imagens);
        return imagens.Select(Mapeador.ParaResposta).ToList();
    }

    public async Task Remover(int id)
    {
        var imagem = await _imagens.BuscarPorId(id)
            ?? throw new ExcecaoDeRecursoNaoEncontrado("Imagem não encontrada.");

        await _imagens.Remover(imagem);
    }
}
