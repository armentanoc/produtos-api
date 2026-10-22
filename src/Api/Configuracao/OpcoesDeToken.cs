namespace Api.Configuracao;

public class OpcoesDeToken
{
    public string Emissor { get; set; } = "";
    public string Publico { get; set; } = "";
    public string Chave { get; set; } = "";
    public int ExpiraEmMinutos { get; set; } = 120;
}
