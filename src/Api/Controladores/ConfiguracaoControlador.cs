using Microsoft.Extensions.Options;

namespace Api.Controladores;

[ApiController]
[Route("api/configuracao")]
[Tags("Configuração")]
public class ConfiguracaoControlador : ControllerBase
{
    private readonly OpcoesDoCatalogo _opcoes;

    public ConfiguracaoControlador(IOptions<OpcoesDoCatalogo> opcoes)
    {
        _opcoes = opcoes.Value;
    }

    /// <summary>Título e subtítulo do catálogo, lidos do appsettings.</summary>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(ConfiguracaoResposta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status401Unauthorized)]
    public ActionResult<ConfiguracaoResposta> Obter()
    {
        return Ok(new ConfiguracaoResposta
        {
            Titulo = _opcoes.Titulo,
            Subtitulo = _opcoes.Subtitulo
        });
    }
}
