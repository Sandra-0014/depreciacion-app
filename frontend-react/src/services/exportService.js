const API_URL = '/api/export'

export async function exportarPdf(activoId) {
  const token = localStorage.getItem('token')

  if (!token) {
    throw new Error(
      'Tu sesión no está disponible. Inicia sesión nuevamente.',
    )
  }

  const respuesta = await fetch(`${API_URL}/pdf/${activoId}`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (respuesta.status === 401) {
    throw new Error('Tu sesión venció. Inicia sesión nuevamente.')
  }

  if (respuesta.status === 404) {
    throw new Error('No se encontró el activo que deseas exportar.')
  }

  if (!respuesta.ok) {
    throw new Error(
      `No se pudo generar el PDF. Error ${respuesta.status}.`,
    )
  }

  const pdf = await respuesta.blob()

  const url = URL.createObjectURL(pdf)

  const enlace = document.createElement('a')
  enlace.href = url
  enlace.download = `depreciacion-${activoId}.pdf`

  document.body.appendChild(enlace)
  enlace.click()
  enlace.remove()

  URL.revokeObjectURL(url)
}