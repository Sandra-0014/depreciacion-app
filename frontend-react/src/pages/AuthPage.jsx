import { useState } from 'react'
import LoginForm from '../components/auth/LoginForm'
import RegistroForm from '../components/auth/RegistroForm'

function AuthPage({ onSesionIniciada }) {
  const [modo, setModo] = useState('login')
  const [mensaje, setMensaje] = useState('')

  const cambiarModo = (nuevoModo) => {
    setModo(nuevoModo)
    setMensaje('')
  }

  const registroExitoso = () => {
    setModo('login')
    setMensaje('Usuario registrado correctamente. Ya puedes iniciar sesión.')
  }

  return (
    <main className="pagina-autenticacion">
      <section className="presentacion">
        <div className="contenido-presentacion">
          <span className="marca">Depreciación App</span>

          <h1>Administra el valor de tus activos</h1>

          <p>
            Registra productos y calcula su depreciación de manera rápida,
            organizada y segura.
          </p>

          <div className="decoracion" aria-hidden="true">
            <div className="circulo circulo-grande"></div>
            <div className="circulo circulo-mediano"></div>
            <div className="circulo circulo-pequeno"></div>
          </div>
        </div>
      </section>

      <section className="contenedor-formulario">
        <div className="tarjeta">
          <div className="selector">
            <button
              type="button"
              className={modo === 'login' ? 'activo' : ''}
              onClick={() => cambiarModo('login')}
            >
              Iniciar sesión
            </button>

            <button
              type="button"
              className={modo === 'registro' ? 'activo' : ''}
              onClick={() => cambiarModo('registro')}
            >
              Registrarse
            </button>
          </div>

          {mensaje && <p className="mensaje exito">{mensaje}</p>}

          {modo === 'login' ? (
            <LoginForm onSesionIniciada={onSesionIniciada} />
          ) : (
            <RegistroForm onRegistroExitoso={registroExitoso} />
          )}
        </div>
      </section>
    </main>
  )
}

export default AuthPage