namespace Depreciacion.Domain.Entities;

public class DetalleDepreciacion
{
    public int Id { get; set; }
    public int ActivoId { get; set; }
    public int NumeroPeriodo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int MesesPeriodo { get; set; }
    public decimal DepreciacionPeriodo { get; set; }
    public decimal DepreciacionAcumulada { get; set; }
    public decimal ValorLibros { get; set; }
}