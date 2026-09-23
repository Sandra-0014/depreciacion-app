namespace Pdf.Domain;

public interface IPdfGenerator
{
    byte[] Generar(TablaDepreciacionInfo tabla);
}