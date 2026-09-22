using Auth.Domain.Entities;

namespace Auth.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerarToken(Usuario usuario);
}