using Aplicacao.Servicos;
using Dominio.Entidades;
using Dominio.Seguranca;
using TestesUnitarios.Apoio;

namespace TestesUnitarios;

public class ServicoDeAutenticacaoTestes
{
    [Fact]
    public async Task Login_certo_entra()
    {
        var (hash, sal) = ServicoDeSenha.Gerar("admin@123");
        var usuarios = new RepositorioDeUsuariosFalso
        {
            Usuario = new Usuario { Login = "admin", SenhaHash = hash, Sal = sal }
        };
        var servico = new ServicoDeAutenticacao(usuarios);

        Assert.True(await servico.CredenciaisValidas("admin", "admin@123"));
    }

    [Fact]
    public async Task Login_errado_nao_entra()
    {
        var servico = new ServicoDeAutenticacao(new RepositorioDeUsuariosFalso());

        Assert.False(await servico.CredenciaisValidas("admin", "admin@123"));
    }
}
