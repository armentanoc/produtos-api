namespace Aplicacao.Contratos;

public class CadastrarImagens
{
    [Range(1, int.MaxValue, ErrorMessage = "Informe o produto.")]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "Informe ao menos uma URL.")]
    [MinLength(1, ErrorMessage = "Informe ao menos uma URL.")]
    public List<string>? Urls { get; set; }
}
