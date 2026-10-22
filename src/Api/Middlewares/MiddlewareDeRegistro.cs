using System.Diagnostics;

namespace Api.Middlewares;

public class MiddlewareDeRegistro
{
    private readonly RequestDelegate _proximo;
    private readonly ILogger<MiddlewareDeRegistro> _logger;

    public MiddlewareDeRegistro(RequestDelegate proximo, ILogger<MiddlewareDeRegistro> logger)
    {
        _proximo = proximo;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        var metodo = contexto.Request.Method;
        var caminho = contexto.Request.Path;
        _logger.LogInformation("Requisição {Metodo} {Caminho}", metodo, caminho);

        var relogio = Stopwatch.StartNew();
        try
        {
            await _proximo(contexto);
        }
        finally
        {
            relogio.Stop();
            var status = contexto.Response.StatusCode;
            var milissegundos = relogio.ElapsedMilliseconds;

            if (status >= 500)
            {
                _logger.LogError(
                    "Resposta {Metodo} {Caminho} {Status} em {Milissegundos} ms",
                    metodo, caminho, status, milissegundos);
            }
            else if (status >= 400)
            {
                _logger.LogWarning(
                    "Resposta {Metodo} {Caminho} {Status} em {Milissegundos} ms",
                    metodo, caminho, status, milissegundos);
            }
            else
            {
                _logger.LogInformation(
                    "Resposta {Metodo} {Caminho} {Status} em {Milissegundos} ms",
                    metodo, caminho, status, milissegundos);
            }
        }
    }
}
