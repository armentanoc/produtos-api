using Dominio.Entidades;

namespace Dominio.Repositorios;

public interface IRepositorioDeImagens
{
    Task<IReadOnlyList<Imagem>> ListarPorProduto(int produtoId);
    Task<Imagem?> BuscarPorId(int id);
    Task CadastrarVarias(IReadOnlyList<Imagem> imagens);
    Task Remover(Imagem imagem);
}
