using Depreciacion.Domain.Entities;
namespace Depreciacion.Domain.Services;
public class CalculoDepreciacionService
{
    public List<DetalleDepreciacion> Calcular(Activo activo)
    {
        int vidaUtilAnios = CategoriaInfo.VidaUtilAnios(activo.Categoria);
        decimal valorResidual = activo.ValorCompra * CategoriaInfo.PorcentajeValorResidual;
        decimal baseDepreciable = activo.ValorCompra - valorResidual;
        decimal depreciacionMensual = (baseDepreciable / vidaUtilAnios) / 12m;

        activo.ValorResidual = Math.Round(valorResidual, 2);

        DateTime finVidaUtil = activo.FechaAdquisicion.AddYears(vidaUtilAnios);
        var detalles = new List<DetalleDepreciacion>();

        DateTime inicioPeriodo = activo.FechaAdquisicion;
        decimal acumulada = 0m;
        int numero = 1;

        while (inicioPeriodo < activo.FechaCalculo)
        {
            DateTime finPeriodoCompleto = inicioPeriodo.AddYears(1);
            DateTime finPeriodo = finPeriodoCompleto < activo.FechaCalculo
                ? finPeriodoCompleto
                : activo.FechaCalculo;

            int mesesPeriodo = MesesEntre(inicioPeriodo, finPeriodo);
            decimal depreciacionPeriodo;

            if (inicioPeriodo >= finVidaUtil)
            {
                depreciacionPeriodo = 0m; // ya cumplió su vida útil
            }
            else if (finPeriodo > finVidaUtil)
            {
                int mesesHastaFinVidaUtil = MesesEntre(inicioPeriodo, finVidaUtil);
                depreciacionPeriodo = depreciacionMensual * mesesHastaFinVidaUtil;
            }
            else
            {
                depreciacionPeriodo = depreciacionMensual * mesesPeriodo;
            }

            if (acumulada + depreciacionPeriodo > baseDepreciable)
                depreciacionPeriodo = baseDepreciable - acumulada;
            if (depreciacionPeriodo < 0) depreciacionPeriodo = 0;

            acumulada += depreciacionPeriodo;

            detalles.Add(new DetalleDepreciacion
            {
                NumeroPeriodo = numero,
                FechaInicio = inicioPeriodo,
                FechaFin = finPeriodo,
                MesesPeriodo = mesesPeriodo,
                DepreciacionPeriodo = Math.Round(depreciacionPeriodo, 2),
                DepreciacionAcumulada = Math.Round(acumulada, 2),
                ValorLibros = Math.Round(activo.ValorCompra - acumulada, 2)
            });

            inicioPeriodo = finPeriodo;
            numero++;
        }

        return detalles;
    }

    private static int MesesEntre(DateTime inicio, DateTime fin)
    {
        int meses = (fin.Year - inicio.Year) * 12 + (fin.Month - inicio.Month);
        if (fin.Day < inicio.Day) meses--;
        return Math.Max(meses, 0);
    }
}