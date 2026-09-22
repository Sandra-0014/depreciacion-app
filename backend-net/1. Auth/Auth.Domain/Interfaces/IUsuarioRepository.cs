using Auth.Domain.Entities;

namespace Auth.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<bool> ExisteCorreoAsync(string correo);

    Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);

    Task<Usuario?> ObtenerPorNombreUsuarioAsync(
        string nombreUsuario
    );

    Task AgregarAsync(Usuario usuario);

    Task GuardarCambiosAsync();
}