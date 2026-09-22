const API_URL = '/api/depreciacion'

async function leerRespuesta(respuesta) {
  const texto = await respuesta.text()

  if (!texto) {
    return null
  }

  try {
    return JSON.parse(texto)
  } catch {
    return null
  }
}

export async function calcularDepreciacion(datosActivo) {
  const token = localStorage.getItem('token')

  if (!token) {
    throw new Error('Tu sesión no está disponible. Inicia sesión nuevamente.')
  }

  const respuesta = await fetch(`${API_URL}/calcular`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(datosActivo),
  })

  const datos = await leerRespuesta(respuesta)

  if (respuesta.status === 401) {
    throw new Error('Tu sesión venció. Inicia sesión nuevamente.')
  }

  if (!respuesta.ok) {
    const erroresValidacion = datos?.errors
      ? Object.values(datos.errors).flat().join(' ')
      : ''

    throw new Error(
      datos?.mensaje ||
        erroresValidacion ||
        `No se pudo calcular la depreciación. Error ${respuesta.status}.`,
    )
  }

  return datos
}

export async function obtenerDepreciacion(id) {
  const token = localStorage.getItem('token')

  if (!token) {
    throw new Error('Tu sesión no está disponible. Inicia sesión nuevamente.')
  }

  const respuesta = await fetch(`${API_URL}/${id}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  const datos = await leerRespuesta(respuesta)

  if (respuesta.status === 401) {
    throw new Error('Tu sesión venció. Inicia sesión nuevamente.')
  }

  if (respuesta.status === 404) {
    throw new Error('No se encontró el cálculo solicitado.')
  }

  if (!respuesta.ok) {
    throw new Error(`No se pudo consultar el cálculo. Error ${respuesta.status}.`)
  }

  return datos
}