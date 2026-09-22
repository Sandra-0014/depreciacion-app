using System.ComponentModel.DataAnnotations;

namespace Auth.Domain.Entities;

public class Usuario
{
    [Key]
    [MaxLength(150)]
    public string Correo { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;
}