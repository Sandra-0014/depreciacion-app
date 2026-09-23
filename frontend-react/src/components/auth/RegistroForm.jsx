import { useState } from 'react'
import { registrarUsuario } from '../../services/authService'

const registroInicial = {
  nombre: '',
  apellido: '',
  correo: '',
  nombreUsuario: '',
  contrasena: '',
}

function RegistroForm({ onRegistroExitoso }) {
  const [registro, setRegistro] = useState(registroInicial)
  const [mensaje, setMensaje] = useState('')
  const [cargando, setCargando] = useState(false)

  const actualizarCampo = (evento) => {
    const { name, value } = evento.target

    setRegistro((datosAnteriores) => ({
      ...datosAnteriores,
      [name]: value,
    }))
  }

  const enviarFormulario = async (evento) => {
    evento.preventDefault()
    setCargando(true)
    setMensaje('')

    try {
      await registrarUsuario(registro)
      setRegistro(registroInicial)
      onRegistroExitoso()
    } catch (error) {
      setMensaje(error.message)
    } finally {
      setCargando(false)
    }
  }

  return (
    <>
      <div className="encabezado-formulario">
        <h2>Crear una cuenta</h2>
        <p>Completa tus datos para registrarte.</p>
      </div>

      <form onSubmit={enviarFormulario}>
        <div className="fila-campos">
          <div>
            <label htmlFor="nombre">Nombre</label>
            <input
              id="nombre"
              name="nombre"
              type="text"
              value={registro.nombre}
              onChange={actualizarCampo}
              placeholder="Tu nombre"
              required
            />
          </div>

          <div>
            <label htmlFor="apellido">Apellido</label>
            <input
              id="apellido"
              name="apellido"
              type="text"
              value={registro.apellido}
              onChange={actualizarCampo}
              placeholder="Tu apellido"
              required
            />
          </div>
        </div>

        <label htmlFor="correo">Correo electrónico</label>
        <input
          id="correo"
          name="correo"
          type="email"
          value={registro.correo}
          onChange={actualizarCampo}
          placeholder="correo@ejemplo.com"
          autoComplete="email"
          required
        />

        <label htmlFor="nombreUsuario">Nombre de usuario</label>
        <input
          id="nombreUsuario"
          name="nombreUsuario"
          type="text"
          value={registro.nombreUsuario}
          onChange={actualizarCampo}
          placeholder="Elige un usuario"
          autoComplete="username"
          required
        />

        <label htmlFor="contrasena">Contraseña</label>
        <input
          id="contrasena"
          name="contrasena"
          type="password"
          value={registro.contrasena}
          onChange={actualizarCampo}
          placeholder="Mínimo 8 caracteres"
          autoComplete="new-password"
          minLength="8"
          required
        />

        {mensaje && <p className="mensaje error">{mensaje}</p>}

        <button
          type="submit"
          className="boton-principal"
          disabled={cargando}
        >
          {cargando ? 'Registrando...' : 'Crear cuenta'}
        </button>
      </form>
    </>
  )
}

export default RegistroForm