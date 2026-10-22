using Dominio.Entidades;
using Dominio.Repositorios;

namespace TestesUnitarios.Apoio;

public class RepositorioDeProdutosFalso : IRepositorioDeProdutos
{
    private readonly List<Produto> _produtos = [];
    private int _proximoId = 1;

    public void Guardar(Produto produto) => _produtos.Add(produto);

    public Task<IReadOnlyList<Produto>> Listar(bool somenteAtivos)
    {
        IEnumerable<Produto> lista = _produtos;
        if (somenteAtivos)
            lista = lista.Where(produto => produto.EstaAtivo);
        return Task.FromResult<IReadOnlyList<Produto>>(lista.ToList());
    }

    public Task<Produto?> BuscarPorId(int id) =>
        Task.FromResult(_produtos.FirstOrDefault(produto => produto.Id == id));

    public Task<bool> Existe(int id) =>
        Task.FromResult(_produtos.Any(produto => produto.Id == id));

    public Task Cadastrar(Produto produto)
    {
        produto.Id = _proximoId++;
        _produtos.Add(produto);
        return Task.CompletedTask;
    }

    public Task Atualizar(Produto produto) => Task.CompletedTask;

    public Task Remover(Produto produto)
    {
        _produtos.Remove(produto);
        return Task.CompletedTask;
    }
}
