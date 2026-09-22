using System.Security.Claims;
using Depreciacion.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Depreciacion.API.Controllers;

[ApiController]
[Route("api/depreciacion")]
[Authorize]
public class DepreciacionController : ControllerBase
{
    private readonly DepreciacionAppService _appService;

    public DepreciacionController(DepreciacionAppService appService) => _appService = appService;

    private string UsuarioId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User.FindFirstValue("sub")
        ?? throw new InvalidOperationException("Token sin identificador de usuario.");

    [HttpPost("calcular")]
    public async Task<IActionResult> Calcular([FromBody] CrearActivoRequest request)
    {
        var resultado = await _appService.CrearYCalcularAsync(request, UsuarioId);
        return StatusCode(StatusCodes.Status201Created, resultado);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Obtener(int id)
    {
        var resultado = await _appService.ObtenerAsync(id, UsuarioId);
        if (resultado is null) return NotFound();
        return Ok(resultado);
    }
}