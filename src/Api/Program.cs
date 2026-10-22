var builder = WebApplication.CreateBuilder(args);

var porta = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(porta))
    builder.WebHost.UseUrls($"http://0.0.0.0:{porta}");

var chaveJwt = builder.Configuration["Jwt:Chave"] ?? "";
if (chaveJwt.Length < 32)
    throw new InvalidOperationException("Jwt:Chave precisa ter pelo menos 32 caracteres. Ajuste o appsettings ou a vari\u00e1vel Jwt__Chave.");

if (builder.Configuration.GetValue("Jwt:ExpiraEmMinutos", 0) < 1)
    throw new InvalidOperationException("Jwt:ExpiraEmMinutos precisa ser pelo menos 1.");

var loginInicial = builder.Configuration["UsuarioInicial:Login"];
var senhaInicial = builder.Configuration["UsuarioInicial:Senha"];
if (string.IsNullOrWhiteSpace(loginInicial) || string.IsNullOrWhiteSpace(senhaInicial))
    throw new InvalidOperationException("Defina UsuarioInicial:Login e UsuarioInicial:Senha no appsettings.");

ServicosDaApi.Adicionar(builder, chaveJwt);

var app = builder.Build();
await InicializadorDoBanco.Preparar(app.Services, loginInicial, senhaInicial);
app.Logger.LogInformation("SQLite em {Conexao}", app.Configuration.GetConnectionString("Catalogo"));

var titulo = app.Configuration["Catalogo:Titulo"] ?? "Cat\u00e1logo de Produtos";
var swaggerHabilitado = app.Configuration.GetValue("Swagger:Habilitado", true);

app.UseMiddleware<MiddlewareDeRegistro>();
if (swaggerHabilitado)
{
    app.UseSwagger();
    app.UseSwaggerUI(opcoes => opcoes.DocumentTitle = titulo);
}

app.UseCors(PoliticaDeCors.Nome);
app.UseMiddleware<MiddlewareDeExcecao>();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/saude", () => Results.Ok(new { status = "ok" })).AllowAnonymous();
app.MapControllers();
app.Run();
