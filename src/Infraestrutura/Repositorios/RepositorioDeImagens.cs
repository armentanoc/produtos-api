namespace Infraestrutura.Repositorios;

public class RepositorioDeImagens : IRepositorioDeImagens
{
    private readonly ContextoDeDados _contexto;

    public RepositorioDeImagens(ContextoDeDados contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<Imagem>> ListarPorProduto(int produtoId)
    {
        return await _contexto.Imagens
            .AsNoTracking()
            .Where(imagem => imagem.ProdutoId == produtoId)
            .OrderBy(imagem => imagem.Id)
            .ToListAsync();
    }

    public Task<Imagem?> BuscarPorId(int id)
    {
        return _contexto.Imagens.FirstOrDefaultAsync(imagem => imagem.Id == id);
    }

    public async Task CadastrarVarias(IReadOnlyList<Imagem> imagens)
    {
        var agora = DateTime.UtcNow;
        foreach (var imagem in imagens)
            imagem.MarcarCadastro(agora);

        _contexto.Imagens.AddRange(imagens);
        await _contexto.SaveChangesAsync();
    }

    public async Task Remover(Imagem imagem)
    {
        _contexto.Imagens.Remove(imagem);
        await _contexto.SaveChangesAsync();
    }
}
