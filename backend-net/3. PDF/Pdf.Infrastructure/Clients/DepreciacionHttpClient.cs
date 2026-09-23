using System.Net.Http.Json;
using Pdf.Application;
using Pdf.Domain;

namespace Pdf.Infrastructure;

public class DepreciacionHttpClient : IDepreciacionClient
{
    private readonly HttpClient _httpClient;

    public DepreciacionHttpClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<TablaDepreciacionInfo?> ObtenerTablaAsync(int activoId, string tokenJwt)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/depreciacion/{activoId}");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenJwt);

        var respuesta = await _httpClient.SendAsync(request);
        if (!respuesta.IsSuccessStatusCode) return null;

        var dto = await respuesta.Content.ReadFromJsonAsync<TablaDepreciacionResponseDto>();
        if (dto is null) return null;

        return new TablaDepreciacionInfo
        {
            Nombre = dto.Nombre,
            ValorCompra = dto.ValorCompra,
            ValorResidual = dto.ValorResidual,
            Detalles = dto.Detalles.Select(d => new DetallePeriodoInfo
            {
                NumeroPeriodo = d.NumeroPeriodo,
                FechaInicio = d.FechaInicio,
                FechaFin = d.FechaFin,
                DepreciacionPeriodo = d.DepreciacionPeriodo,
                DepreciacionAcumulada = d.DepreciacionAcumulada,
                ValorLibros = d.ValorLibros
            }).ToList()
        };
    }

    // Clases privadas SOLO para deserializar la respuesta JSON de Depreciacion.API.
    // No son el modelo de Domain — son el "espejo" del contrato externo.
    private class TablaDepreciacionResponseDto
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal ValorCompra { get; set; }
        public decimal ValorResidual { get; set; }
        public List<DetalleDepreciacionResponseDto> Detalles { get; set; } = new();
    }

    private class DetalleDepreciacionResponseDto
    {
        public int NumeroPeriodo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal DepreciacionPeriodo { get; set; }
        public decimal DepreciacionAcumulada { get; set; }
        public decimal ValorLibros { get; set; }
    }
}