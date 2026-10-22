using Dominio.Entidades;

namespace Dominio.Repositorios;

public interface IRepositorioDeUsuarios
{
    Task<Usuario?> BuscarPorLogin(string login);
}
