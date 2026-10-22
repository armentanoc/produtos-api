using Infraestrutura.Repositorios;

namespace Infraestrutura;

public static class InjecaoDeDependencia
{
    public static IServiceCollection AdicionarInfraestrutura(this IServiceCollection servicos, string conexao)
    {
        servicos.AddDbContext<ContextoDeDados>(opcoes => opcoes.UseSqlite(conexao));
        servicos.AddScoped<IRepositorioDeProdutos, RepositorioDeProdutos>();
        servicos.AddScoped<IRepositorioDeImagens, RepositorioDeImagens>();
        servicos.AddScoped<IRepositorioDeUsuarios, RepositorioDeUsuarios>();
        return servicos;
    }
}
