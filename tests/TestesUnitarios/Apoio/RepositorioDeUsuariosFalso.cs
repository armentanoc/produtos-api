using Dominio.Entidades;
using Dominio.Repositorios;

namespace TestesUnitarios.Apoio;

public class RepositorioDeUsuariosFalso : IRepositorioDeUsuarios
{
    public Usuario? Usuario { get; set; }

    public Task<Usuario?> BuscarPorLogin(string login) =>
        Task.FromResult(Usuario?.Login == login ? Usuario : null);
}
