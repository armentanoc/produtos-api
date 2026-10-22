namespace Infraestrutura.Repositorios;

public class RepositorioDeUsuarios : IRepositorioDeUsuarios
{
    private readonly ContextoDeDados _contexto;

    public RepositorioDeUsuarios(ContextoDeDados contexto)
    {
        _contexto = contexto;
    }

    public Task<Usuario?> BuscarPorLogin(string login)
    {
        return _contexto.Usuarios.FirstOrDefaultAsync(usuario => usuario.Login == login);
    }
}
