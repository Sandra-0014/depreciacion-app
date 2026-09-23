using Pdf.Domain;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Pdf.Infrastructure;

public class QuestPdfGenerator : IPdfGenerator
{
    public byte[] Generar(TablaDepreciacionInfo tabla)
    {
        var documento = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);

                page.Header().Text($"Tabla de Depreciación — {tabla.Nombre}")
                    .FontSize(16).Bold();

                page.Content().Column(columna =>
                {
                    columna.Item().Text($"Valor de compra: ${tabla.ValorCompra:N2}");
                    columna.Item().Text($"Valor residual: ${tabla.ValorResidual:N2}");
                    columna.Item().PaddingTop(10);

                    columna.Item().Table(tablaPdf =>
                    {
                        tablaPdf.ColumnsDefinition(columnas =>
                        {
                            columnas.RelativeColumn();
                            columnas.RelativeColumn(1.5f);
                            columnas.RelativeColumn(1.5f);
                            columnas.RelativeColumn(1.5f);
                            columnas.RelativeColumn(1.5f);
                            columnas.RelativeColumn(1.5f);
                        });

                        tablaPdf.Header(encabezado =>
                        {
                            encabezado.Cell().Text("Período").Bold();
                            encabezado.Cell().Text("Inicio").Bold();
                            encabezado.Cell().Text("Fin").Bold();
                            encabezado.Cell().Text("Deprec.").Bold();
                            encabezado.Cell().Text("Acumulada").Bold();
                            encabezado.Cell().Text("Valor Libros").Bold();
                        });

                        foreach (var detalle in tabla.Detalles)
                        {
                            tablaPdf.Cell().Text(detalle.NumeroPeriodo.ToString());
                            tablaPdf.Cell().Text(detalle.FechaInicio.ToString("dd/MM/yyyy"));
                            tablaPdf.Cell().Text(detalle.FechaFin.ToString("dd/MM/yyyy"));
                            tablaPdf.Cell().Text($"${detalle.DepreciacionPeriodo:N2}");
                            tablaPdf.Cell().Text($"${detalle.DepreciacionAcumulada:N2}");
                            tablaPdf.Cell().Text($"${detalle.ValorLibros:N2}");
                        }
                    });
                });

                page.Footer().AlignCenter().Text(texto =>
                {
                    texto.Span("Generado por Depreciación App — ");
                    texto.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                });
            });
        });

        return documento.GeneratePdf();
    }
}