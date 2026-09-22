using System.ComponentModel.DataAnnotations;
using Depreciacion.Domain;

namespace Depreciacion.Application;

public class CrearActivoRequest
{
    [Required, MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public CategoriaActivo Categoria { get; set; }

    [Required, Range(0.01, double.MaxValue)]
    public decimal ValorCompra { get; set; }

    [Required]
    public DateTime FechaAdquisicion { get; set; }

    [Required]
    public DateTime FechaCalculo { get; set; }
}