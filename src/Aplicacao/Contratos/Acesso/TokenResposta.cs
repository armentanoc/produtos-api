namespace Aplicacao.Contratos;

public class TokenResposta
{
    public string Token { get; set; } = "";
    public string Tipo { get; set; } = "Bearer";
    public DateTime ExpiraEm { get; set; }
}
