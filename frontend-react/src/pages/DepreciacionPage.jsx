import { useState } from 'react'
import FormularioActivo from '../components/depreciacion/FormularioActivo'
import TablaDepreciacion from '../components/depreciacion/TablaDepreciacion'

function DepreciacionPage({ usuario, onCerrarSesion }) {
  const [resultado, setResultado] = useState(null)

  return (
    <main className="pagina-depreciacion">
      <header className="barra-superior">
        <div>
          <span className="marca">Depreciación App</span>
          <p className="saludo-usuario">
            Bienvenida, {usuario.nombreUsuario}
          </p>
        </div>

        <button
          type="button"
          className="boton-cerrar-sesion"
          onClick={onCerrarSesion}
        >
          Cerrar sesión
        </button>
      </header>

      <section className="encabezado-depreciacion">
        <span className="etiqueta-seccion">Gestión de activos</span>
        <h1>Calcula la depreciación de tus activos</h1>
        <p>
          Registra los datos del activo para obtener su valor residual y la
          depreciación correspondiente a cada periodo.
        </p>
      </section>

      <div className="contenido-depreciacion">
        <FormularioActivo onCalculoRealizado={setResultado} />
        <TablaDepreciacion resultado={resultado} />
      </div>
    </main>
  )
}

export default DepreciacionPage