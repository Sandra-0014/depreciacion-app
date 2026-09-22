using Depreciacion.Domain;
using Depreciacion.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Depreciacion.Infrastructure;

public class ActivoRepository : IActivoRepository
{
    private readonly DepreciacionDbContext _context;

    public ActivoRepository(DepreciacionDbContext context) => _context = context;

    public Task<Activo?> ObtenerPorIdAsync(int id, string usuarioId) =>
        _context.Activos
            .Include(a => a.Detalles)
            .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == usuarioId);

    public Task<List<Activo>> ObtenerPorUsuarioAsync(string usuarioId) =>
        _context.Activos
            .Where(a => a.UsuarioId == usuarioId)
            .OrderByDescending(a => a.FechaAdquisicion)
            .ToListAsync();

    public async Task AgregarAsync(Activo activo) =>
        await _context.Activos.AddAsync(activo);

    public Task GuardarCambiosAsync() => _context.SaveChangesAsync();
}