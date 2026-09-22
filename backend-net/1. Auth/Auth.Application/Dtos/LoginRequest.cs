using System.ComponentModel.DataAnnotations;

namespace Auth.Application.Dtos;

public class LoginRequest
{
    [Required]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required]
    public string Contrasena { get; set; } = string.Empty;
}