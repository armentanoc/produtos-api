namespace Dominio.Excecoes;

public class ExcecaoDeRequisicaoInvalida : Exception
{
    public ExcecaoDeRequisicaoInvalida(string mensagem) : base(mensagem)
    {
    }
}
