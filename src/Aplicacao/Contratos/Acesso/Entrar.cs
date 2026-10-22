namespace Aplicacao.Contratos;

public class Entrar
{
    [Required(ErrorMessage = "Informe o login.")]
    public string? Login { get; set; }

    [Required(ErrorMessage = "Informe a senha.")]
    public string? Senha { get; set; }
}
