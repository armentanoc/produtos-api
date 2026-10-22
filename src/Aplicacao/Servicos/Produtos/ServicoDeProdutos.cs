namespace Aplicacao.Servicos;

public class ServicoDeProdutos : IServicoDeProdutos
{
    private readonly IRepositorioDeProdutos _produtos;

    public ServicoDeProdutos(IRepositorioDeProdutos produtos)
    {
        _produtos = produtos;
    }

    public async Task<IReadOnlyList<ProdutoResposta>> Listar(bool somenteAtivos)
    {
        var produtos = await _produtos.Listar(somenteAtivos);
        return produtos.Select(Mapeador.ParaResposta).ToList();
    }

    public async Task<ProdutoResposta> Buscar(int id, bool incluirInativos)
    {
        var produto = await Obter(id);
        if (!produto.EstaAtivo && !incluirInativos)
            throw new ExcecaoDeRecursoNaoEncontrado("Produto não encontrado.");

        return Mapeador.ParaResposta(produto);
    }

    public async Task<ProdutoResposta> Cadastrar(CadastrarProduto pedido)
    {
        var produto = new Produto
        {
            EstaAtivo = true,
            Imagens = Validador.Urls(pedido.Urls, obrigatorio: false)
                .Select(url => new Imagem { Url = url })
                .ToList()
        };
        Preencher(produto, pedido);
        await _produtos.Cadastrar(produto);
        return Mapeador.ParaResposta(produto);
    }

    public async Task<ProdutoResposta> Atualizar(int id, AtualizarProduto pedido)
    {
        var produto = await Obter(id);
        Preencher(produto, pedido);
        produto.EstaAtivo = pedido.EstaAtivo ?? true;
        await _produtos.Atualizar(produto);
        return Mapeador.ParaResposta(produto);
    }

    public async Task Remover(int id)
    {
        await _produtos.Remover(await Obter(id));
    }

    private async Task<Produto> Obter(int id)
    {
        return await _produtos.BuscarPorId(id)
            ?? throw new ExcecaoDeRecursoNaoEncontrado("Produto não encontrado.");
    }

    private static void Preencher(Produto produto, DadosDoProduto pedido)
    {
        produto.Nome = Validador.Texto(pedido.Nome, "Informe o nome.", 120);
        produto.Descricao = Validador.Texto(pedido.Descricao, "Informe a descrição.", 2000);
    }
}
