using Auth.Service.Data;
using Auth.Service.Dtos;
using Auth.Service.Models;
using Auth.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Service.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly JwtService _jwtService;

    public AuthController(
        AuthDbContext context,
        IPasswordHasher<Usuario> passwordHasher,
        JwtService jwtService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registrar(
        [FromBody] RegistroRequest request)
    {
        string correo = request.Correo.Trim().ToLowerInvariant();
        string nombreUsuario = request.NombreUsuario.Trim();

        bool correoExiste = await _context.Usuarios
            .AnyAsync(usuario => usuario.Correo == correo);

        if (correoExiste)
        {
            return Conflict(new
            {
                mensaje = "El correo ya está registrado."
            });
        }

        bool nombreUsuarioExiste = await _context.Usuarios
            .AnyAsync(usuario =>
                usuario.NombreUsuario == nombreUsuario
            );

        if (nombreUsuarioExiste)
        {
            return Conflict(new
            {
                mensaje = "El nombre de usuario ya está registrado."
            });
        }

        var usuario = new Usuario
        {
            Correo = correo,
            Nombre = request.Nombre.Trim(),
            Apellido = request.Apellido.Trim(),
            NombreUsuario = nombreUsuario
        };

        usuario.PasswordHash = _passwordHasher.HashPassword(
            usuario,
            request.Contrasena
        );

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created, new
        {
            mensaje = "Usuario registrado correctamente."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        string nombreUsuario = request.NombreUsuario.Trim();

        Usuario? usuario = await _context.Usuarios
            .FirstOrDefaultAsync(usuario =>
                usuario.NombreUsuario == nombreUsuario
            );

        if (usuario is null)
        {
            return Unauthorized(new
            {
                mensaje = "Usuario o contraseña incorrectos."
            });
        }

        PasswordVerificationResult resultado =
            _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.PasswordHash,
                request.Contrasena
            );

        if (resultado == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new
            {
                mensaje = "Usuario o contraseña incorrectos."
            });
        }

        if (resultado ==
            PasswordVerificationResult.SuccessRehashNeeded)
        {
            usuario.PasswordHash = _passwordHasher.HashPassword(
                usuario,
                request.Contrasena
            );

            await _context.SaveChangesAsync();
        }

        string token = _jwtService.GenerarToken(usuario);

        return Ok(new AuthResponse
        {
            Token = token,
            NombreUsuario = usuario.NombreUsuario,
            Correo = usuario.Correo
        });
    }
}