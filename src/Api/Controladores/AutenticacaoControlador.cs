namespace Api.Controladores;

[ApiController]
[Route("api/autenticacao")]
[Tags("Autenticação")]
public class AutenticacaoControlador : ControllerBase
{
    private readonly IServicoDeAutenticacao _autenticacao;
    private readonly EmissorDeToken _emissor;
    private readonly ILogger<AutenticacaoControlador> _logger;

    public AutenticacaoControlador(
        IServicoDeAutenticacao autenticacao,
        EmissorDeToken emissor,
        ILogger<AutenticacaoControlador> logger)
    {
        _autenticacao = autenticacao;
        _emissor = emissor;
        _logger = logger;
    }

    /// <summary>Confere login e senha e devolve o token Bearer.</summary>
    /// <remarks>
    /// Exemplo: { "login": "admin", "senha": "admin@123" }.
    /// Copie o campo token e cole no botão Authorize, sem escrever a palavra Bearer.
    /// </remarks>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(TokenResposta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResposta>> Entrar([FromBody] Entrar pedido)
    {
        var login = pedido.Login?.Trim() ?? "";
        if (!await _autenticacao.CredenciaisValidas(login, pedido.Senha ?? ""))
        {
            _logger.LogWarning("Falha de autenticação para o login {Login}.", login);
            return Unauthorized(new RespostaDeErro
            {
                Mensagem = "Login ou senha inválidos.",
                Status = StatusCodes.Status401Unauthorized
            });
        }

        _logger.LogInformation("Login {Login} autenticado.", login);
        return Ok(_emissor.Emitir(login));
    }
}
