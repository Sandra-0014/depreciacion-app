const formatoMoneda = new Intl.NumberFormat('es-EC', {
  style: 'currency',
  currency: 'USD',
  minimumFractionDigits: 2,
})

const formatearFecha = (fecha) => {
  if (!fecha) return '-'

  return new Date(fecha).toLocaleDateString('es-EC', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
  })
}

function TablaDepreciacion({ resultado }) {
  if (!resultado) {
    return (
      <section className="tarjeta-depreciacion resultado-vacio">
        <span className="etiqueta-seccion">Resultado</span>
        <h2>Tabla de depreciación</h2>
        <p>
          Completa el formulario para calcular y mostrar la depreciación del
          activo.
        </p>
      </section>
    )
  }

  const detalles = resultado.detalles ?? []
  const valorDepreciable =
    Number(resultado.valorCompra) - Number(resultado.valorResidual)

  return (
    <section className="tarjeta-depreciacion">
      <header className="encabezado-seccion">
        <div>
          <span className="etiqueta-seccion">Resultado</span>
          <h2>{resultado.nombre}</h2>
        </div>

        <span className="identificador-activo">
          Activo #{resultado.activoId}
        </span>
      </header>

      <div className="resumen-depreciacion">
        <article>
          <span>Valor de compra</span>
          <strong>{formatoMoneda.format(resultado.valorCompra)}</strong>
        </article>

        <article>
          <span>Valor residual</span>
          <strong>{formatoMoneda.format(resultado.valorResidual)}</strong>
        </article>

        <article>
          <span>Valor depreciable</span>
          <strong>{formatoMoneda.format(valorDepreciable)}</strong>
        </article>
      </div>

      {detalles.length === 0 ? (
        <p className="mensaje-vacio">
          No existen periodos de depreciación para mostrar.
        </p>
      ) : (
        <div className="contenedor-tabla">
          <table className="tabla-depreciacion">
            <thead>
              <tr>
                <th>Periodo</th>
                <th>Fecha inicial</th>
                <th>Fecha final</th>
                <th>Meses</th>
                <th>Depreciación</th>
                <th>Acumulada</th>
                <th>Valor en libros</th>
              </tr>
            </thead>

            <tbody>
              {detalles.map((detalle) => (
                <tr key={detalle.numeroPeriodo}>
                  <td>{detalle.numeroPeriodo}</td>
                  <td>{formatearFecha(detalle.fechaInicio)}</td>
                  <td>{formatearFecha(detalle.fechaFin)}</td>
                  <td>{detalle.mesesPeriodo}</td>
                  <td>
                    {formatoMoneda.format(detalle.depreciacionPeriodo)}
                  </td>
                  <td>
                    {formatoMoneda.format(detalle.depreciacionAcumulada)}
                  </td>
                  <td>{formatoMoneda.format(detalle.valorLibros)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </section>
  )
}

export default TablaDepreciacion