using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerarToken(Usuario usuario)
    {
        string key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "No se configuró Jwt:Key."
            );

        string issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "No se configuró Jwt:Issuer."
            );

        string audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "No se configuró Jwt:Audience."
            );

        int minutos = int.Parse(
            _configuration["Jwt:ExpirationMinutes"] ?? "60"
        );

        var claims = new[]
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                usuario.Correo
            ),
            new Claim(
                JwtRegisteredClaimNames.Email,
                usuario.Correo
            ),
            new Claim(
                JwtRegisteredClaimNames.UniqueName,
                usuario.NombreUsuario
            ),
            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()
            )
        };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key)
        );

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(minutos),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}