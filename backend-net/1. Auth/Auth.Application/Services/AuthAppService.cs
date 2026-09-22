using Auth.Application.Dtos;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Auth.Application.Services;

public class AuthAppService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthAppService(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher<Usuario> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<(bool Exitoso, string Mensaje)> RegistrarAsync(
        RegistroRequest request)
    {
        string correo = request.Correo.Trim().ToLowerInvariant();
        string nombreUsuario = request.NombreUsuario.Trim();

        if (await _usuarioRepository.ExisteCorreoAsync(correo))
        {
            return (
                false,
                "El correo ya está registrado."
            );
        }

        if (await _usuarioRepository
            .ExisteNombreUsuarioAsync(nombreUsuario))
        {
            return (
                false,
                "El nombre de usuario ya está registrado."
            );
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

        await _usuarioRepository.AgregarAsync(usuario);
        await _usuarioRepository.GuardarCambiosAsync();

        return (
            true,
            "Usuario registrado correctamente."
        );
    }

    public async Task<AuthResponse?> LoginAsync(
        LoginRequest request)
    {
        string nombreUsuario = request.NombreUsuario.Trim();

        Usuario? usuario =
            await _usuarioRepository.ObtenerPorNombreUsuarioAsync(
                nombreUsuario
            );

        if (usuario is null)
        {
            return null;
        }

        PasswordVerificationResult resultado =
            _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.PasswordHash,
                request.Contrasena
            );

        if (resultado == PasswordVerificationResult.Failed)
        {
            return null;
        }

        if (resultado ==
            PasswordVerificationResult.SuccessRehashNeeded)
        {
            usuario.PasswordHash = _passwordHasher.HashPassword(
                usuario,
                request.Contrasena
            );

            await _usuarioRepository.GuardarCambiosAsync();
        }

        return new AuthResponse
        {
            Token = _jwtTokenGenerator.GenerarToken(usuario),
            NombreUsuario = usuario.NombreUsuario,
            Correo = usuario.Correo
        };
    }
}