using Auth.Application.Dtos;
using Auth.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthAppService _authAppService;

    public AuthController(AuthAppService authAppService)
    {
        _authAppService = authAppService;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registrar(
        [FromBody] RegistroRequest request)
    {
        var resultado =
            await _authAppService.RegistrarAsync(request);

        if (!resultado.Exitoso)
        {
            return Conflict(new
            {
                mensaje = resultado.Mensaje
            });
        }

        return StatusCode(StatusCodes.Status201Created, new
        {
            mensaje = resultado.Mensaje
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        AuthResponse? respuesta =
            await _authAppService.LoginAsync(request);

        if (respuesta is null)
        {
            return Unauthorized(new
            {
                mensaje = "Usuario o contraseña incorrectos."
            });
        }

        return Ok(respuesta);
    }
}