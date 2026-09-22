using Depreciacion.Domain;
using Depreciacion.Domain.Entities;

namespace Depreciacion.Application;

public class DepreciacionAppService
{
    private readonly IActivoRepository _repositorio;
    private readonly CalculoDepreciacionService _calculo;

    public DepreciacionAppService(IActivoRepository repositorio, CalculoDepreciacionService calculo)
    {
        _repositorio = repositorio;
        _calculo = calculo;
    }

    public async Task<TablaDepreciacionResponse> CrearYCalcularAsync(CrearActivoRequest request, string usuarioId)
    {
        var activo = new Activo
        {
            UsuarioId = usuarioId,
            Nombre = request.Nombre.Trim(),
            Categoria = request.Categoria,
            ValorCompra = request.ValorCompra,
            FechaAdquisicion = request.FechaAdquisicion,
            FechaCalculo = request.FechaCalculo
        };

        activo.Detalles = _calculo.Calcular(activo);

        await _repositorio.AgregarAsync(activo);
        await _repositorio.GuardarCambiosAsync();

        return MapearRespuesta(activo);
    }

    public async Task<TablaDepreciacionResponse?> ObtenerAsync(int id, string usuarioId)
    {
        var activo = await _repositorio.ObtenerPorIdAsync(id, usuarioId);
        return activo is null ? null : MapearRespuesta(activo);
    }

    private static TablaDepreciacionResponse MapearRespuesta(Activo activo) => new()
    {
        ActivoId = activo.Id,
        Nombre = activo.Nombre,
        ValorCompra = activo.ValorCompra,
        ValorResidual = activo.ValorResidual,
        Detalles = activo.Detalles.Select(d => new DetalleDepreciacionResponse
        {
            NumeroPeriodo = d.NumeroPeriodo,
            FechaInicio = d.FechaInicio,
            FechaFin = d.FechaFin,
            MesesPeriodo = d.MesesPeriodo,
            DepreciacionPeriodo = d.DepreciacionPeriodo,
            DepreciacionAcumulada = d.DepreciacionAcumulada,
            ValorLibros = d.ValorLibros
        }).ToList()
    };
}