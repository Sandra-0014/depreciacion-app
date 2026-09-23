using Pdf.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pdf.API.Controllers;

[ApiController]
[Route("api/export")]
[Authorize]
public class ExportController : ControllerBase
{
    private readonly ExportAppService _exportAppService;

    public ExportController(ExportAppService exportAppService) => _exportAppService = exportAppService;

    [HttpGet("pdf/{activoId}")]
    public async Task<IActionResult> ExportarPdf(int activoId)
    {
        string? token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        if (string.IsNullOrEmpty(token)) return Unauthorized();

        byte[]? pdf = await _exportAppService.GenerarPdfAsync(activoId, token);
        if (pdf is null) return NotFound(new { mensaje = "Activo no encontrado o sin acceso." });

        return File(pdf, "application/pdf", $"depreciacion-{activoId}.pdf");
    }
}