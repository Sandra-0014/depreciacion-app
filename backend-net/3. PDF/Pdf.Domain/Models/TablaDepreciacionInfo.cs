namespace Pdf.Domain;

public class TablaDepreciacionInfo
{
    public string Nombre { get; set; } = string.Empty;
    public decimal ValorCompra { get; set; }
    public decimal ValorResidual { get; set; }
    public List<DetallePeriodoInfo> Detalles { get; set; } = new();
}