namespace Aplicacao.Contratos;

public class AtualizarProduto : DadosDoProduto
{
    [Required(ErrorMessage = "Informe se o produto está ativo.")]
    public bool? EstaAtivo { get; set; }
}
