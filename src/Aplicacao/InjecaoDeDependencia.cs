namespace Aplicacao;

public static class InjecaoDeDependencia
{
    public static IServiceCollection AdicionarAplicacao(this IServiceCollection servicos)
    {
        servicos.AddScoped<IServicoDeProdutos, ServicoDeProdutos>();
        servicos.AddScoped<IServicoDeImagens, ServicoDeImagens>();
        servicos.AddScoped<IServicoDeAutenticacao, ServicoDeAutenticacao>();
        return servicos;
    }
}
