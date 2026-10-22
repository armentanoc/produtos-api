namespace Infraestrutura.Contexto;

public class ContextoDeDados : DbContext
{
    public ContextoDeDados(DbContextOptions<ContextoDeDados> opcoes) : base(opcoes)
    {
    }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Imagem> Imagens => Set<Imagem>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        modelo.Entity<Produto>(entidade =>
        {
            entidade.ToTable("produtos");
            entidade.HasKey(produto => produto.Id);
            entidade.Property(produto => produto.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entidade.Property(produto => produto.Nome).HasColumnName("nome").IsRequired().HasMaxLength(120);
            entidade.Property(produto => produto.Descricao).HasColumnName("descricao").IsRequired().HasMaxLength(2000);
            entidade.Property(produto => produto.DataCadastro).HasColumnName("data_cadastro").IsRequired();
            entidade.Property(produto => produto.DataAtualizacao).HasColumnName("data_atualizacao").IsRequired();
            entidade.Property(produto => produto.EstaAtivo).HasColumnName("esta_ativo").HasDefaultValue(true);
            entidade.HasMany(produto => produto.Imagens)
                .WithOne(imagem => imagem.Produto)
                .HasForeignKey(imagem => imagem.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelo.Entity<Imagem>(entidade =>
        {
            entidade.ToTable("imagens");
            entidade.HasKey(imagem => imagem.Id);
            entidade.Property(imagem => imagem.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entidade.Property(imagem => imagem.ProdutoId).HasColumnName("produto_id").IsRequired();
            entidade.Property(imagem => imagem.Url).HasColumnName("url").IsRequired().HasMaxLength(2000);
            entidade.Property(imagem => imagem.DataCadastro).HasColumnName("data_cadastro").IsRequired();
        });

        modelo.Entity<Usuario>(entidade =>
        {
            entidade.ToTable("usuarios");
            entidade.HasKey(usuario => usuario.Id);
            entidade.Property(usuario => usuario.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entidade.Property(usuario => usuario.Login).HasColumnName("login").IsRequired().HasMaxLength(80);
            entidade.HasIndex(usuario => usuario.Login).IsUnique();
            entidade.Property(usuario => usuario.SenhaHash).HasColumnName("senha_hash").IsRequired().HasMaxLength(200);
            entidade.Property(usuario => usuario.Sal).HasColumnName("sal").IsRequired().HasMaxLength(100);
            entidade.Property(usuario => usuario.DataCadastro).HasColumnName("data_cadastro").IsRequired();
        });
    }
}
