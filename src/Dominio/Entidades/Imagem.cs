namespace Dominio.Entidades;

public class Imagem
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string Url { get; set; } = "";
    public DateTime DataCadastro { get; set; }
    public Produto? Produto { get; set; }

    public void MarcarCadastro(DateTime agora) => DataCadastro = agora;
}
