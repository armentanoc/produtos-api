namespace Dominio.Excecoes;

public class ExcecaoDeRecursoNaoEncontrado : Exception
{
    public ExcecaoDeRecursoNaoEncontrado(string mensagem) : base(mensagem)
    {
    }
}
