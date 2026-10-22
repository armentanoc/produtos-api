namespace Aplicacao.Servicos;

public interface IServicoDeProdutos
{
    Task<IReadOnlyList<ProdutoResposta>> Listar(bool somenteAtivos);
    Task<ProdutoResposta> Buscar(int id, bool incluirInativos);
    Task<ProdutoResposta> Cadastrar(CadastrarProduto pedido);
    Task<ProdutoResposta> Atualizar(int id, AtualizarProduto pedido);
    Task Remover(int id);
}
