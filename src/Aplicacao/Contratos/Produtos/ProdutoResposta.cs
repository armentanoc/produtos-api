namespace Aplicacao.Contratos;

public class ProdutoResposta
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Descricao { get; set; } = "";
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public bool EstaAtivo { get; set; }
    public List<ImagemResposta> Imagens { get; set; } = [];
}
