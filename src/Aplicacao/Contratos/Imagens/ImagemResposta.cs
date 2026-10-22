namespace Aplicacao.Contratos;

public class ImagemResposta
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string Url { get; set; } = "";
    public DateTime DataCadastro { get; set; }
}
