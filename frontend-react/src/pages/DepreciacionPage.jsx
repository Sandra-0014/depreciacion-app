import { useState } from 'react'
import FormularioActivo from '../components/depreciacion/FormularioActivo'
import TablaDepreciacion from '../components/depreciacion/TablaDepreciacion'
import { calcularDepreciacion } from '../services/depreciacionService'
import '../depreciacion.css'

function DepreciacionPage({ usuario, onCerrarSesion }) {
  const [resultado, setResultado] = useState(null)
  const [mensaje, setMensaje] = useState('')
  const [cargando, setCargando] = useState(false)

  const realizarCalculo = async (datosActivo) => {
    setCargando(true)
    setMensaje('')

    try {
      const datos = await calcularDepreciacion(datosActivo)

      setResultado(datos)
      setMensaje('Activo registrado y depreciación calculada correctamente.')

      return true
    } catch (error) {
      setMensaje(error.message)
      return false
    } finally {
      setCargando(false)
    }
  }

  return (
    <main className="pagina-depreciacion">
      <header className="barra-superior">
        <div>
          <span className="marca marca-panel">Depreciación App</span>
        </div>

        <div className="usuario-panel">
          <div>
            <strong>{usuario.nombreUsuario}</strong>
            <span>{usuario.correo}</span>
          </div>

          <button
            type="button"
            className="boton-cerrar-sesion"
            onClick={onCerrarSesion}
          >
            Cerrar sesión
          </button>
        </div>
      </header>

      <section className="contenido-depreciacion">
        <div className="encabezado-pagina">
          <span className="etiqueta-seccion">Panel de activos</span>

          <h1>Calcula la depreciación de tus activos</h1>

          <p>
            Registra la información del activo y consulta su depreciación,
            valor acumulado y valor actual en libros.
          </p>
        </div>

        {mensaje && (
          <p
            className={
              resultado
                ? 'mensaje-global mensaje-exito'
                : 'mensaje-global mensaje-error'
            }
          >
            {mensaje}
          </p>
        )}

        <div className="distribucion-depreciacion">
          <FormularioActivo
            onCalcular={realizarCalculo}
            cargando={cargando}
          />

          <TablaDepreciacion resultado={resultado} />
        </div>
      </section>
    </main>
  )
}

export default DepreciacionPage