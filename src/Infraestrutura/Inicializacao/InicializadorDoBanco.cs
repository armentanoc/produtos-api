namespace Infraestrutura.Inicializacao;

public static class InicializadorDoBanco
{
    private const string Foto = "https://cdn.gbmedia.com.br/images/w_900,f_auto,fl_progressive,q_auto:best/v1/";

    public static async Task Preparar(IServiceProvider servicos, string loginInicial, string senhaInicial)
    {
        using var escopo = servicos.CreateScope();
        var contexto = escopo.ServiceProvider.GetRequiredService<ContextoDeDados>();
        var agora = DateTime.UtcNow;

        GarantirPasta(contexto.Database.GetDbConnection().ConnectionString);
        await contexto.Database.EnsureCreatedAsync();

        if (!await contexto.Usuarios.AnyAsync())
        {
            var (hash, sal) = ServicoDeSenha.Gerar(senhaInicial);
            var usuario = new Usuario
            {
                Login = loginInicial.Trim(),
                SenhaHash = hash,
                Sal = sal
            };
            usuario.MarcarCadastro(agora);
            contexto.Usuarios.Add(usuario);
        }

        if (!await contexto.Produtos.AnyAsync())
            contexto.Produtos.AddRange(Exemplos(agora));

        await contexto.SaveChangesAsync();
    }

    private static void GarantirPasta(string conexao)
    {
        const string marcador = "Data Source=";
        var indice = conexao.IndexOf(marcador, StringComparison.OrdinalIgnoreCase);
        if (indice < 0)
            return;

        var caminho = conexao[(indice + marcador.Length)..].Split(';')[0].Trim().Trim('"');
        var pasta = Path.GetDirectoryName(caminho);
        if (!string.IsNullOrEmpty(pasta))
            Directory.CreateDirectory(pasta);
    }

    private static List<Produto> Exemplos(DateTime agora)
    {
        return
        [
            Exemplo("Lily Eau de Parfum 75ml",
                "Eau de parfum floral da linha Lily. Frasco de 75 ml com a assinatura de lírio.",
                agora,
                Foto + "imagens/product/B89331/ff8a787b-0320-40d3-950f-c5422b054a94-lily-eau-de-parfum-75ml-b77524.jpg",
                Foto + "imagens/product/B89331/db912a83-9fb2-4491-bd9a-daeaebd824a6-bot-77524-lily-perfume-edp-ambientada.jpg"),
            Exemplo("Floratta Rose Desodorante Colônia 75ml",
                "Colônia floral da linha Floratta Rose, no frasco rosa de 75 ml.",
                agora,
                Foto + "imagens/product/B48313/8557651e-b7f3-4243-a1ee-cbe2405c9211-bot-48313-floratta-rose-desodorante-colonia-01.jpg",
                Foto + "imagens/product/B48313/39d3d5a5-9bc9-4a49-93ef-08930a4f590c-floratta-rose-b48313.jpg"),
            Exemplo("Eudora Siàge Nutri Rosé Shampoo 250ml",
                "Shampoo da linha Siàge Nutri Rosé, da Eudora, em embalagem de 250 ml.",
                agora,
                Foto + "imagens/products/E53435/SIAGE_NUTRI_ROSE_SHAMPOO_E53435_PRINCIPAL_1.jpg"),
            Exemplo("Eudora Instance Açaí e Bambu Condicionador 300ml",
                "Condicionador da linha Instance Açaí e Bambu, da Eudora, em embalagem de 300 ml.",
                agora,
                Foto + "imagens/products/E50094/INSTANCE_ACAI_BAMBU_CONC_300ml_E50094.jpg"),
            Exemplo("Quem Disse, Berenice? Batom Hidratante Rosa Armarosa 3,8g",
                "Batom hidratante rosa da Quem Disse, Berenice?, em embalagem de 3,8 g.",
                agora,
                Foto + "imagens/product/Q75325/9b519c48-060f-41c2-b7cb-de9bae17418a-batom-hidratante-armarosa.png"),
            Exemplo("Quem Disse, Berenice? Máscara para Cílios Volume e Definição 10g",
                "Máscara de cílios da Quem Disse, Berenice?, em embalagem de 10 g.",
                agora,
                Foto + "imagens/product/Q55986/91e4db43-f5fb-4fc2-82fe-b5cec3e34d41-qdb-55986-volume-e-definicao-mascara-de-cilios-frontal-02.jpg"),
            Exemplo("Truss Ultra Hydration Shampoo 300ml",
                "Shampoo da linha Truss Ultra Hydration, em embalagem de 300 ml.",
                agora,
                Foto + "imagens/product/36550/90b0c40b-1033-4607-9286-f8ddc8df11a5-truss-ultra-hydration-shampoo-300ml.png"),
            Exemplo("Truss Night Spa Sérum 250ml",
                "Sérum noturno da linha Truss Night Spa, em embalagem de 250 ml.",
                agora,
                Foto + "imagens/product/52866/70e9e46a-8f07-4c26-931a-a8ad5aea8e0d-truss-night-spa-serum-de-tratamento-noturno-250ml.png")
        ];
    }

    private static Produto Exemplo(string nome, string descricao, DateTime agora, params string[] urls)
    {
        var produto = new Produto
        {
            Nome = nome,
            Descricao = descricao,
            EstaAtivo = true,
            Imagens = urls.Select(url => new Imagem { Url = url }).ToList()
        };
        produto.MarcarCadastro(agora);
        return produto;
    }
}
