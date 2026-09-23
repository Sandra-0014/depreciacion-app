const API_URL = '/api/auth'

const leerRespuesta = async (respuesta) => {
  const texto = await respuesta.text()

  if (!texto) {
    return {}
  }

  try {
    return JSON.parse(texto)
  } catch {
    throw new Error('El servidor devolvió una respuesta inválida.')
  }
}

const enviarSolicitud = async (ruta, datos) => {
  const respuesta = await fetch(`${API_URL}/${ruta}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(datos),
  })

  const contenido = await leerRespuesta(respuesta)

  if (!respuesta.ok) {
    throw new Error(
      contenido.mensaje ||
        contenido.title ||
        'No se pudo completar la solicitud.',
    )
  }

  return contenido
}

export const iniciarSesion = (credenciales) =>
  enviarSolicitud('login', credenciales)

export const registrarUsuario = (usuario) =>
  enviarSolicitud('registro', usuario)