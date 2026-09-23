using Pdf.Domain;

namespace Pdf.Application;

public class ExportAppService
{
    private readonly IDepreciacionClient _depreciacionClient;
    private readonly IPdfGenerator _pdfGenerator;

    public ExportAppService(IDepreciacionClient depreciacionClient, IPdfGenerator pdfGenerator)
    {
        _depreciacionClient = depreciacionClient;
        _pdfGenerator = pdfGenerator;
    }

    public async Task<byte[]?> GenerarPdfAsync(int activoId, string tokenJwt)
    {
        TablaDepreciacionInfo? tabla = await _depreciacionClient.ObtenerTablaAsync(activoId, tokenJwt);
        if (tabla is null) return null;

        return _pdfGenerator.Generar(tabla);
    }
}