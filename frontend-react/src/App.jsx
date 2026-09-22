import { useState } from 'react'
import DepreciacionPage from './pages/DepreciacionPage'
import './App.css'

const API_URL = '/api/auth'

const registroInicial = {
  nombre: '',
  apellido: '',
  correo: '',
  nombreUsuario: '',
  contrasena: '',
}

function App() {
  const [modo, setModo] = useState('login')
  const [login, setLogin] = useState({
    nombreUsuario: '',
    contrasena: '',
  })
  const [registro, setRegistro] = useState(registroInicial)
  const [usuario, setUsuario] = useState(() => {
    const usuarioGuardado = localStorage.getItem('usuario')
    return usuarioGuardado ? JSON.parse(usuarioGuardado) : null
  })
  const [mensaje, setMensaje] = useState('')
  const [tipoMensaje, setTipoMensaje] = useState('')
  const [cargando, setCargando] = useState(false)

  const cambiarModo = (nuevoModo) => {
    setModo(nuevoModo)
    setMensaje('')
    setTipoMensaje('')
  }

  const actualizarLogin = (evento) => {
    const { name, value } = evento.target

    setLogin((datosAnteriores) => ({
      ...datosAnteriores,
      [name]: value,
    }))
  }

  const actualizarRegistro = (evento) => {
    const { name, value } = evento.target

    setRegistro((datosAnteriores) => ({
      ...datosAnteriores,
      [name]: value,
    }))
  }

  const iniciarSesion = async (evento) => {
    evento.preventDefault()
    setCargando(true)
    setMensaje('')

    try {
      const respuesta = await fetch(`${API_URL}/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(login),
      })

      const datos = await respuesta.json()

      if (!respuesta.ok) {
        throw new Error(datos.mensaje || 'No se pudo iniciar sesión.')
      }

      localStorage.setItem('token', datos.token)
      localStorage.setItem(
        'usuario',
        JSON.stringify({
          nombreUsuario: datos.nombreUsuario,
          correo: datos.correo,
        }),
      )

      setUsuario({
        nombreUsuario: datos.nombreUsuario,
        correo: datos.correo,
      })
    } catch (error) {
      setTipoMensaje('error')
      setMensaje(error.message)
    } finally {
      setCargando(false)
    }
  }

  const registrarUsuario = async (evento) => {
    evento.preventDefault()
    setCargando(true)
    setMensaje('')

    try {
      const respuesta = await fetch(`${API_URL}/registro`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(registro),
      })

      const datos = await respuesta.json()

      if (!respuesta.ok) {
        throw new Error(datos.mensaje || 'No se pudo completar el registro.')
      }

      setRegistro(registroInicial)
      setModo('login')
      setTipoMensaje('exito')
      setMensaje('Usuario registrado correctamente. Ya puedes iniciar sesión.')
    } catch (error) {
      setTipoMensaje('error')
      setMensaje(error.message)
    } finally {
      setCargando(false)
    }
  }

  const cerrarSesion = () => {
    localStorage.removeItem('token')
    localStorage.removeItem('usuario')
    setUsuario(null)
    setLogin({
      nombreUsuario: '',
      contrasena: '',
    })
    setMensaje('')
  }

  if (usuario) {
  return (
    <DepreciacionPage
      usuario={usuario}
      onCerrarSesion={cerrarSesion}
    />
  )
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

          {modo === 'login' ? (
            <>
              <div className="encabezado-formulario">
                <h2>Iniciar sesión</h2>
                <p>Ingresa tus datos para continuar.</p>
              </div>

              <form onSubmit={iniciarSesion}>
                <label htmlFor="loginUsuario">Nombre de usuario</label>
                <input
                  id="loginUsuario"
                  name="nombreUsuario"
                  type="text"
                  value={login.nombreUsuario}
                  onChange={actualizarLogin}
                  placeholder="Ingresa tu usuario"
                  autoComplete="username"
                  required
                />

                <label htmlFor="loginContrasena">Contraseña</label>
                <input
                  id="loginContrasena"
                  name="contrasena"
                  type="password"
                  value={login.contrasena}
                  onChange={actualizarLogin}
                  placeholder="Ingresa tu contraseña"
                  autoComplete="current-password"
                  required
                />

                {mensaje && (
                  <p className={`mensaje ${tipoMensaje}`}>{mensaje}</p>
                )}

                <button
                  type="submit"
                  className="boton-principal"
                  disabled={cargando}
                >
                  {cargando ? 'Ingresando...' : 'Ingresar'}
                </button>
              </form>
            </>
          ) : (
            <>
              <div className="encabezado-formulario">
                <h2>Crear una cuenta</h2>
                <p>Completa tus datos para registrarte.</p>
              </div>

              <form onSubmit={registrarUsuario}>
                <div className="fila-campos">
                  <div>
                    <label htmlFor="nombre">Nombre</label>
                    <input
                      id="nombre"
                      name="nombre"
                      type="text"
                      value={registro.nombre}
                      onChange={actualizarRegistro}
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
                      onChange={actualizarRegistro}
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
                  onChange={actualizarRegistro}
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
                  onChange={actualizarRegistro}
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
                  onChange={actualizarRegistro}
                  placeholder="Mínimo 8 caracteres"
                  autoComplete="new-password"
                  minLength="8"
                  required
                />

                {mensaje && (
                  <p className={`mensaje ${tipoMensaje}`}>{mensaje}</p>
                )}

                <button
                  type="submit"
                  className="boton-principal"
                  disabled={cargando}
                >
                  {cargando ? 'Registrando...' : 'Crear cuenta'}
                </button>
              </form>
            </>
          )}
        </div>
      </section>
    </main>
  )
}

export default App