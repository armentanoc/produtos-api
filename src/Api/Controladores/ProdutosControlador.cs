namespace Api.Controladores;

[ApiController]
[Route("api/produtos")]
[Tags("Produtos")]
public class ProdutosControlador : ControllerBase
{
    private readonly IServicoDeProdutos _servico;

    public ProdutosControlador(IServicoDeProdutos servico)
    {
        _servico = servico;
    }

    /// <summary>Lista os produtos ativos. incluirInativos=true inclui os desativados.</summary>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProdutoResposta>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<ProdutoResposta>>> Listar([FromQuery] bool incluirInativos = false)
    {
        return Ok(await _servico.Listar(somenteAtivos: !incluirInativos));
    }

    /// <summary>Busca um produto e as URLs de imagem dele.</summary>
    [Authorize]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProdutoResposta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProdutoResposta>> Buscar(int id)
    {
        return Ok(await _servico.Buscar(id, incluirInativos: true));
    }

    /// <summary>Cadastra um produto. O id e as datas são gerados pela API. Urls é opcional.</summary>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ProdutoResposta), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProdutoResposta>> Cadastrar([FromBody] CadastrarProduto pedido)
    {
        var produto = await _servico.Cadastrar(pedido);
        return CreatedAtAction(nameof(Buscar), new { id = produto.Id }, produto);
    }

    /// <summary>Atualiza nome, descrição e se o produto continua ativo.</summary>
    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProdutoResposta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(RespostaDeErro), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProdutoResposta>> Atualizar(int id, [FromBody] AtualizarProduto pedido)
    {
        return Ok(await _servico.Atualizar(id, pedido));
    }

    /// <summary>Apaga o produto e as imagens ligadas a ele.</summary>
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
