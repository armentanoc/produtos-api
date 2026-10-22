namespace Aplicacao.Servicos;

public interface IServicoDeAutenticacao
{
    Task<bool> CredenciaisValidas(string login, string senha);
}
