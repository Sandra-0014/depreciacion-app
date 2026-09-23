namespace Pdf.Domain;

public class DetallePeriodoInfo
{
    public int NumeroPeriodo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal DepreciacionPeriodo { get; set; }
    public decimal DepreciacionAcumulada { get; set; }
    public decimal ValorLibros { get; set; }
}