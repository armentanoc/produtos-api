namespace Api.Inicializacao;

public static class ServicosDaApi
{
    public static void Adicionar(WebApplicationBuilder builder, string chaveJwt)
    {
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opcoes => JsonPadrao.Aplicar(opcoes.SerializerOptions));
        builder.Services.Configure<OpcoesDoCatalogo>(builder.Configuration.GetSection("Catalogo"));
        builder.Services.Configure<OpcoesDeToken>(builder.Configuration.GetSection("Jwt"));
        builder.Services.AddSingleton<EmissorDeToken>();
        builder.Services.AdicionarAplicacao();
        builder.Services.AdicionarInfraestrutura(BancoSqlite.DefinirConexao(builder));
        PoliticaDeCors.Adicionar(builder.Services, builder.Configuration);
        AdicionarJwt(builder, chaveJwt);
        builder.Services.AddAuthorization(opcoes =>
        {
            opcoes.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
        builder.Services.AddControllers().AddJsonOptions(opcoes => JsonPadrao.Aplicar(opcoes.JsonSerializerOptions));
        builder.Services.Configure<ApiBehaviorOptions>(opcoes =>
        {
            opcoes.InvalidModelStateResponseFactory = CriarRespostaDeValidacao;
        });
        AdicionarSwagger(builder);
    }

    private static IActionResult CriarRespostaDeValidacao(ActionContext contexto)
    {
        var erros = contexto.ModelState.Values
            .SelectMany(item => item?.Errors ?? [])
            .Select(erro => string.IsNullOrWhiteSpace(erro.ErrorMessage) ? "A requisi\u00e7\u00e3o est\u00e1 inv\u00e1lida." : erro.ErrorMessage)
            .Distinct()
            .ToArray();

        return new BadRequestObjectResult(new RespostaDeErro
        {
            Mensagem = erros.Length == 0 ? "A requisi\u00e7\u00e3o est\u00e1 inv\u00e1lida." : string.Join(" ", erros),
            Status = StatusCodes.Status400BadRequest
        });
    }

    private static void AdicionarJwt(WebApplicationBuilder builder, string chaveJwt)
    {
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opcoes =>
            {
                opcoes.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Emissor"],
                    ValidAudience = builder.Configuration["Jwt:Publico"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt)),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
                opcoes.Events = new JwtBearerEvents
                {
                    OnChallenge = async contexto =>
                    {
                        contexto.HandleResponse();
                        if (contexto.Response.HasStarted)
                            return;

                        contexto.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await contexto.Response.WriteAsJsonAsync(new RespostaDeErro
                        {
                            Mensagem = "Token ausente ou inv\u00e1lido. Gere um em POST /api/autenticacao e cole no Authorize.",
                            Status = StatusCodes.Status401Unauthorized
                        }, JsonPadrao.Opcoes);
                    }
                };
            });
    }

    private static void AdicionarSwagger(WebApplicationBuilder builder)
    {
        var titulo = builder.Configuration["Catalogo:Titulo"] ?? "Cat\u00e1logo de Produtos";
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opcoes =>
        {
            opcoes.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = titulo,
                Version = "v1",
                Description = "Rotas com cadeado exigem o token de POST /api/autenticacao. No Authorize, cole s\u00f3 o token."
            });
            opcoes.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Cole apenas o token, sem a palavra Bearer.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });
            opcoes.OperationFilter<FiltroDeTokenNoSwagger>();
            var xml = Path.Combine(AppContext.BaseDirectory, "Api.xml");
            if (File.Exists(xml))
                opcoes.IncludeXmlComments(xml);
        });
    }
}
