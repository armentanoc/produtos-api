namespace Dominio.Entidades;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Descricao { get; set; } = "";
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public bool EstaAtivo { get; set; } = true;
    public List<Imagem> Imagens { get; set; } = [];

    public void MarcarCadastro(DateTime agora)
    {
        DataCadastro = agora;
        DataAtualizacao = agora;
        foreach (var imagem in Imagens)
            imagem.MarcarCadastro(agora);
    }

    public void MarcarAtualizacao(DateTime agora) => DataAtualizacao = agora;
}
