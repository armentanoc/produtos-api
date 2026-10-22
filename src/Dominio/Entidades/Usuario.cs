namespace Dominio.Entidades;

public class Usuario
{
    public int Id { get; set; }
    public string Login { get; set; } = "";
    public string SenhaHash { get; set; } = "";
    public string Sal { get; set; } = "";
    public DateTime DataCadastro { get; set; }

    public void MarcarCadastro(DateTime agora) => DataCadastro = agora;
}
