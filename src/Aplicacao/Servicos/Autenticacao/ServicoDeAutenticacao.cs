namespace Aplicacao.Servicos;

public class ServicoDeAutenticacao : IServicoDeAutenticacao
{
    private readonly IRepositorioDeUsuarios _usuarios;

    public ServicoDeAutenticacao(IRepositorioDeUsuarios usuarios)
    {
        _usuarios = usuarios;
    }

    public async Task<bool> CredenciaisValidas(string login, string senha)
    {
        var usuario = await _usuarios.BuscarPorLogin(login.Trim());
        var hash = usuario?.SenhaHash ?? ServicoDeSenha.HashFalso;
        var sal = usuario?.Sal ?? ServicoDeSenha.SalFalso;
        var confere = ServicoDeSenha.Confere(senha, hash, sal);
        return usuario is not null && confere;
    }
}
