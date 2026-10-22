namespace Aplicacao.Servicos;

public interface IServicoDeImagens
{
    Task<IReadOnlyList<ImagemResposta>> ListarPorProduto(int produtoId);
    Task<ImagemResposta> Buscar(int id);
    Task<IReadOnlyList<ImagemResposta>> Cadastrar(CadastrarImagens pedido);
    Task Remover(int id);
}
