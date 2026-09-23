const TOKEN_KEY = 'token'
const USER_KEY = 'usuario'

export const guardarSesion = (datos) => {
  const usuario = {
    nombreUsuario: datos.nombreUsuario,
    correo: datos.correo,
  }

  localStorage.setItem(TOKEN_KEY, datos.token)
  localStorage.setItem(USER_KEY, JSON.stringify(usuario))

  return usuario
}

export const obtenerUsuario = () => {
  const contenido = localStorage.getItem(USER_KEY)

  if (!contenido) {
    return null
  }

  try {
    return JSON.parse(contenido)
  } catch {
    cerrarSesion()
    return null
  }
}

export const obtenerToken = () => localStorage.getItem(TOKEN_KEY)

export const cerrarSesion = () => {
  localStorage.removeItem(TOKEN_KEY)
  localStorage.removeItem(USER_KEY)
}