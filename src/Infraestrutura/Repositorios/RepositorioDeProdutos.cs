namespace Infraestrutura.Repositorios;

public class RepositorioDeProdutos : IRepositorioDeProdutos
{
    private readonly ContextoDeDados _contexto;

    public RepositorioDeProdutos(ContextoDeDados contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<Produto>> Listar(bool somenteAtivos)
    {
        IQueryable<Produto> consulta = _contexto.Produtos
            .AsNoTracking()
            .Include(produto => produto.Imagens.OrderBy(imagem => imagem.Id));

        if (somenteAtivos)
            consulta = consulta.Where(produto => produto.EstaAtivo);

        return await consulta.OrderBy(produto => produto.Nome).ToListAsync();
    }

    public Task<Produto?> BuscarPorId(int id)
    {
        return _contexto.Produtos
            .Include(produto => produto.Imagens.OrderBy(imagem => imagem.Id))
            .FirstOrDefaultAsync(produto => produto.Id == id);
    }

    public Task<bool> Existe(int id)
    {
        return _contexto.Produtos.AnyAsync(produto => produto.Id == id);
    }

    public async Task Cadastrar(Produto produto)
    {
        produto.MarcarCadastro(DateTime.UtcNow);

        _contexto.Produtos.Add(produto);
        await _contexto.SaveChangesAsync();
    }

    public async Task Atualizar(Produto produto)
    {
        produto.MarcarAtualizacao(DateTime.UtcNow);
        await _contexto.SaveChangesAsync();
    }

    public async Task Remover(Produto produto)
    {
        _contexto.Produtos.Remove(produto);
        await _contexto.SaveChangesAsync();
    }
}
