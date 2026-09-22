namespace Depreciacion.Domain;

public static class CategoriaInfo
{
    public const decimal PorcentajeValorResidual = 0.10m; // 10%, fijo para las 4 categorías

    public static int VidaUtilAnios(CategoriaActivo categoria) => categoria switch
    {
        CategoriaActivo.EquiposComputoSoftware => 3,
        CategoriaActivo.InstalacionesMaquinariaMuebles => 10,
        CategoriaActivo.VehiculosTransporte => 5,
        CategoriaActivo.InmueblesNaves => 20,
        _ => throw new ArgumentOutOfRangeException(nameof(categoria))
    };
}