import { useState } from 'react'
import { exportarPdf } from '../../services/exportService'

const formatoMoneda = new Intl.NumberFormat('es-EC', {
  style: 'currency',
  currency: 'USD',
  minimumFractionDigits: 2,
})

function formatearFecha(fecha) {
  if (!fecha) {
    return '-'
  }

  const fechaSinHora = fecha.split('T')[0]
  const [anio, mes, dia] = fechaSinHora.split('-')

  return `${dia}/${mes}/${anio}`
}

function TablaDepreciacion({ resultado }) {
  const [exportando, setExportando] = useState(false)
  const [errorExportacion, setErrorExportacion] = useState('')

  const descargarPdf = async () => {
    setExportando(true)
    setErrorExportacion('')

    try {
      await exportarPdf(resultado.activoId)
    } catch (error) {
      setErrorExportacion(error.message)
    } finally {
      setExportando(false)
    }
  }

  if (!resultado) {
    return (
      <section className="tarjeta-depreciacion resultado-vacio">
        <div className="icono-resultado" aria-hidden="true">
          $
        </div>

        <h2>Resultado del cálculo</h2>

        <p>
          Registra un activo para visualizar aquí su tabla de depreciación.
        </p>
      </section>
    )
  }

  return (
    <section className="tarjeta-depreciacion seccion-resultado">
      <div className="encabezado-resultado">
        <div>
          <span className="etiqueta-seccion">Cálculo </span>
          <h2>{resultado.nombre}</h2>
        </div>

        <div className="acciones-resultado">
          <div className="resumen-valores">
            <div>
              <span>Valor de compra</span>
              <strong>{formatoMoneda.format(resultado.valorCompra)}</strong>
            </div>

            <div>
              <span>Valor residual</span>
              <strong>{formatoMoneda.format(resultado.valorResidual)}</strong>
            </div>
          </div>

          <button
            type="button"
            className="boton-exportar"
            onClick={descargarPdf}
            disabled={exportando}
          >
            {exportando ? 'Generando PDF...' : 'Exportar PDF'}
          </button>
        </div>
      </div>

      {errorExportacion && (
        <p className="mensaje-exportacion-error">
          {errorExportacion}
        </p>
      )}

      {resultado.detalles?.length > 0 ? (
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
              {resultado.detalles.map((detalle) => (
                <tr key={detalle.numeroPeriodo}>
                  <td>
                    <span className="numero-periodo">
                      {detalle.numeroPeriodo}
                    </span>
                  </td>

                  <td>{formatearFecha(detalle.fechaInicio)}</td>
                  <td>{formatearFecha(detalle.fechaFin)}</td>
                  <td>{detalle.mesesPeriodo}</td>

                  <td>
                    {formatoMoneda.format(detalle.depreciacionPeriodo)}
                  </td>

                  <td>
                    {formatoMoneda.format(detalle.depreciacionAcumulada)}
                  </td>

                  <td className="valor-libros">
                    {formatoMoneda.format(detalle.valorLibros)}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      ) : (
        <p className="mensaje aviso">
          No se generaron periodos de depreciación para las fechas ingresadas.
        </p>
      )}
    </section>
  )
}

export default TablaDepreciacion