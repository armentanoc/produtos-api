namespace Api.Controladores;

[ApiController]
[Route("api/imagens")]
[Tags("Imagens")]
public class ImagensControlador : ControllerBase
{
    private readonly IServicoDeImagens _servico;

    public ImagensControlador(IServicoDeImagens servico)
    {
        _servico = servico;
    }

    /// <summary>Lista as URLs de imagem de um produto.</summary>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ImagemResposta>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<ImagemResposta>>> Listar([FromQuery] int? produtoId)
    {
        if (produtoId is null)
            return BadRequest(new RespostaDeErro { Mensagem = "Informe o produtoId.", Status = StatusCodes.Status400BadRequest });

        return Ok(await _servico.ListarPorProduto(produtoId.Value));
    }

    /// <summary>Busca uma imagem pelo id.</summary>
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ImagemResposta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ImagemResposta>> Buscar(int id)
    {
        return Ok(await _servico.Buscar(id));
    }

    /// <summary>Cadastra várias URLs para o mesmo produto. Cada URL vira uma linha, com id e data gerados.</summary>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(IReadOnlyList<ImagemResposta>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<ImagemResposta>>> Cadastrar([FromBody] CadastrarImagens pedido)
    {
        var imagens = await _servico.Cadastrar(pedido);
        return Created($"/api/imagens?produtoId={pedido.ProdutoId}", imagens);
    }

    /// <summary>Apaga uma URL de imagem.</summary>
    [Authorize]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id)
    {
        await _servico.Remover(id);
        return NoContent();
    }
}
