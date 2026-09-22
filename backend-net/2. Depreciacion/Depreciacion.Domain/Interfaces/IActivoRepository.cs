using Depreciacion.Domain.Entities;

namespace Depreciacion.Domain;

public interface IActivoRepository
{
    Task<Activo?> ObtenerPorIdAsync(int id, string usuarioId);
    Task<List<Activo>> ObtenerPorUsuarioAsync(string usuarioId);
    Task AgregarAsync(Activo activo);
    Task GuardarCambiosAsync();
}