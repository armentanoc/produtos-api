using Dominio.Excecoes;
using Microsoft.EntityFrameworkCore;

namespace Api.Middlewares;

public class MiddlewareDeExcecao
{
    private readonly RequestDelegate _proximo;
    private readonly ILogger<MiddlewareDeExcecao> _logger;

    public MiddlewareDeExcecao(RequestDelegate proximo, ILogger<MiddlewareDeExcecao> logger)
    {
        _proximo = proximo;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _proximo(contexto);
        }
        catch (Exception excecao) when (excecao is not OperationCanceledException)
        {
            var (status, mensagem, inesperado) = excecao switch
            {
                ExcecaoDeRecursoNaoEncontrado => (StatusCodes.Status404NotFound, excecao.Message, false),
                ExcecaoDeRequisicaoInvalida => (StatusCodes.Status400BadRequest, excecao.Message, false),
                DbUpdateException => (StatusCodes.Status400BadRequest, "Não foi possível gravar os dados.", true),
                _ => (StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado.", true)
            };

            if (inesperado)
                _logger.LogError(excecao, "Falha ao atender {Metodo} {Caminho}", contexto.Request.Method, contexto.Request.Path);
            else
                _logger.LogWarning("Requisição recusada {Status}: {Mensagem}", status, mensagem);

            if (contexto.Response.HasStarted)
                throw;

            contexto.Response.StatusCode = status;
            contexto.Response.ContentType = "application/json; charset=utf-8";
            await contexto.Response.WriteAsJsonAsync(new RespostaDeErro
            {
                Mensagem = mensagem,
                Status = status
            }, JsonPadrao.Opcoes);
        }
    }
}
