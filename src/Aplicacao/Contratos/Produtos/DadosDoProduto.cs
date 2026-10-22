namespace Aplicacao.Contratos;

public class DadosDoProduto
{
    [Required(ErrorMessage = "Informe o nome.")]
    [MaxLength(120, ErrorMessage = "O nome pode ter no máximo 120 caracteres.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Informe a descrição.")]
    [MaxLength(2000, ErrorMessage = "A descrição pode ter no máximo 2000 caracteres.")]
    public string? Descricao { get; set; }
}
