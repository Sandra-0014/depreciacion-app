using Pdf.Domain;

namespace Pdf.Application;

public interface IDepreciacionClient
{
    Task<TablaDepreciacionInfo?> ObtenerTablaAsync(int activoId, string tokenJwt);
}