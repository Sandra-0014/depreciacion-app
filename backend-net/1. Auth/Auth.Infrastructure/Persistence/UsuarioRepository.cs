using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AuthDbContext _context;

    public UsuarioRepository(AuthDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExisteCorreoAsync(string correo)
    {
        return _context.Usuarios.AnyAsync(
            usuario => usuario.Correo == correo
        );
    }

    public Task<bool> ExisteNombreUsuarioAsync(
        string nombreUsuario)
    {
        return _context.Usuarios.AnyAsync(
            usuario => usuario.NombreUsuario == nombreUsuario
        );
    }

    public Task<Usuario?> ObtenerPorNombreUsuarioAsync(
        string nombreUsuario)
    {
        return _context.Usuarios.FirstOrDefaultAsync(
            usuario => usuario.NombreUsuario == nombreUsuario
        );
    }

    public async Task AgregarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}