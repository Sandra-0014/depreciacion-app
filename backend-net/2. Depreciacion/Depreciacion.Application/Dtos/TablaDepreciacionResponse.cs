namespace Depreciacion.Application;

public class DetalleDepreciacionResponse
{
    public int NumeroPeriodo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int MesesPeriodo { get; set; }
    public decimal DepreciacionPeriodo { get; set; }
    public decimal DepreciacionAcumulada { get; set; }
    public decimal ValorLibros { get; set; }
}

public class TablaDepreciacionResponse
{
    public int ActivoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal ValorCompra { get; set; }
    public decimal ValorResidual { get; set; }
    public List<DetalleDepreciacionResponse> Detalles { get; set; } = new();
}