import { useState } from 'react'
import { iniciarSesion } from '../../services/authService'
import { guardarSesion } from '../../services/sessionService'

function LoginForm({ onSesionIniciada }) {
  const [credenciales, setCredenciales] = useState({
    nombreUsuario: '',
    contrasena: '',
  })
  const [mensaje, setMensaje] = useState('')
  const [cargando, setCargando] = useState(false)

  const actualizarCampo = (evento) => {
    const { name, value } = evento.target

    setCredenciales((datosAnteriores) => ({
      ...datosAnteriores,
      [name]: value,
    }))
  }

  const enviarFormulario = async (evento) => {
    evento.preventDefault()
    setCargando(true)
    setMensaje('')

    try {
      const datos = await iniciarSesion(credenciales)
      const usuario = guardarSesion(datos)
      onSesionIniciada(usuario)
    } catch (error) {
      setMensaje(error.message)
    } finally {
      setCargando(false)
    }
  }

  return (
    <>
      <div className="encabezado-formulario">
        <h2>Iniciar sesión</h2>
        <p>Ingresa tus datos para continuar.</p>
      </div>

      <form onSubmit={enviarFormulario}>
        <label htmlFor="loginUsuario">Nombre de usuario</label>
        <input
          id="loginUsuario"
          name="nombreUsuario"
          type="text"
          value={credenciales.nombreUsuario}
          onChange={actualizarCampo}
          placeholder="Ingresa tu usuario"
          autoComplete="username"
          required
        />

        <label htmlFor="loginContrasena">Contraseña</label>
        <input
          id="loginContrasena"
          name="contrasena"
          type="password"
          value={credenciales.contrasena}
          onChange={actualizarCampo}
          placeholder="Ingresa tu contraseña"
          autoComplete="current-password"
          required
        />

        {mensaje && <p className="mensaje error">{mensaje}</p>}

        <button
          type="submit"
          className="boton-principal"
          disabled={cargando}
        >
          {cargando ? 'Ingresando...' : 'Ingresar'}
        </button>
      </form>
    </>
  )
}

export default LoginForm