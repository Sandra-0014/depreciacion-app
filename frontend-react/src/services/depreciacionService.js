import { obtenerToken } from './sessionService'

const API_URL = '/api/depreciacion'

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

const obtenerMensajeError = (contenido) => {
  if (contenido.mensaje) {
    return contenido.mensaje
  }

  if (contenido.errors) {
    const mensajes = Object.values(contenido.errors).flat()

    if (mensajes.length > 0) {
      return mensajes[0]
    }
  }

  return contenido.title || 'No se pudo completar la solicitud.'
}

const realizarSolicitud = async (ruta, opciones = {}) => {
  const token = obtenerToken()

  if (!token) {
    throw new Error('La sesión no es válida. Inicia sesión nuevamente.')
  }

  const respuesta = await fetch(`${API_URL}${ruta}`, {
    ...opciones,
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
      ...opciones.headers,
    },
  })

  const contenido = await leerRespuesta(respuesta)

  if (!respuesta.ok) {
    if (respuesta.status === 401) {
      throw new Error('Tu sesión expiró. Inicia sesión nuevamente.')
    }

    if (respuesta.status === 404) {
      throw new Error('No se encontró el cálculo solicitado.')
    }

    throw new Error(obtenerMensajeError(contenido))
  }

  return contenido
}

export const calcularDepreciacion = (activo) =>
  realizarSolicitud('/calcular', {
    method: 'POST',
    body: JSON.stringify(activo),
  })

export const obtenerDepreciacion = (id) =>
  realizarSolicitud(`/${id}`, {
    method: 'GET',
  })