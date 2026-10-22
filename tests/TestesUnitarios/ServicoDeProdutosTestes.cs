using Aplicacao.Contratos;
using Aplicacao.Servicos;
using Dominio.Entidades;
using Dominio.Excecoes;
using TestesUnitarios.Apoio;

namespace TestesUnitarios;

public class ServicoDeProdutosTestes
{
    [Fact]
    public async Task Cadastrar_sem_nome_recusa()
    {
        var servico = new ServicoDeProdutos(new RepositorioDeProdutosFalso());

        var erro = await Assert.ThrowsAsync<ExcecaoDeRequisicaoInvalida>(() =>
            servico.Cadastrar(new CadastrarProduto
            {
                Nome = "  ",
                Descricao = "Rosa"
            }));

        Assert.Equal("Informe o nome.", erro.Message);
    }

    [Fact]
    public async Task Buscar_inexistente_avisa()
    {
        var servico = new ServicoDeProdutos(new RepositorioDeProdutosFalso());

        await Assert.ThrowsAsync<ExcecaoDeRecursoNaoEncontrado>(() =>
            servico.Buscar(9, incluirInativos: true));
    }

    [Fact]
    public async Task Listar_mostra_somente_ativos()
    {
        var repositorio = new RepositorioDeProdutosFalso();
        repositorio.Guardar(new Produto { Id = 1, Nome = "Lily", Descricao = "Flor", EstaAtivo = true });
        repositorio.Guardar(new Produto { Id = 2, Nome = "Arquivo", Descricao = "Fora", EstaAtivo = false });
        var servico = new ServicoDeProdutos(repositorio);

        var lista = await servico.Listar(somenteAtivos: true);

        Assert.Single(lista);
        Assert.Equal("Lily", lista[0].Nome);
    }
}
