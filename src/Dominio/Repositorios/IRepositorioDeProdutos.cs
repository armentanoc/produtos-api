using Dominio.Entidades;

namespace Dominio.Repositorios;

public interface IRepositorioDeProdutos
{
    Task<IReadOnlyList<Produto>> Listar(bool somenteAtivos);
    Task<Produto?> BuscarPorId(int id);
    Task<bool> Existe(int id);
    Task Cadastrar(Produto produto);
    Task Atualizar(Produto produto);
    Task Remover(Produto produto);
}
