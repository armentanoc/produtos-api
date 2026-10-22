using Dominio.Seguranca;

namespace TestesUnitarios;

public class ServicoDeSenhaTestes
{
    [Fact]
    public void Senha_correta_confere()
    {
        var (hash, sal) = ServicoDeSenha.Gerar("admin@123");

        Assert.True(ServicoDeSenha.Confere("admin@123", hash, sal));
    }

    [Fact]
    public void Senha_errada_nao_confere()
    {
        var (hash, sal) = ServicoDeSenha.Gerar("admin@123");

        Assert.False(ServicoDeSenha.Confere("outra-senha", hash, sal));
    }
}
