namespace Depreciacion.Domain.Entities;

using Depreciacion.Domain;

public class Activo
{
    public int Id { get; set; }
    public string UsuarioId { get; set; } = string.Empty;   // viene del JWT (Sub de Auth)
    public string Nombre { get; set; } = string.Empty;
    public CategoriaActivo Categoria { get; set; }
    public decimal ValorCompra { get; set; }
    public DateTime FechaAdquisicion { get; set; }
    public DateTime FechaCalculo { get; set; }
    public decimal ValorResidual { get; set; }

    public List<DetalleDepreciacion> Detalles { get; set; } = new();
}