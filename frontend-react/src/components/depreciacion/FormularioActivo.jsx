import { useState } from 'react'
import { calcularDepreciacion } from '../../services/depreciacionService'

const fechaActual = new Date().toISOString().split('T')[0]

const activoInicial = {
  nombre: '',
  categoria: '1',
  valorCompra: '',
  fechaAdquisicion: fechaActual,
  fechaCalculo: fechaActual,
}

const categorias = [
  { valor: '1', texto: 'Equipos de cómputo y software — 3 años' },
  { valor: '2', texto: 'Instalaciones, maquinaria y muebles — 10 años' },
  { valor: '3', texto: 'Vehículos y transporte — 5 años' },
  { valor: '4', texto: 'Inmuebles y naves — 20 años' },
]

function FormularioActivo({ onCalculoRealizado }) {
  const [activo, setActivo] = useState(activoInicial)
  const [mensaje, setMensaje] = useState('')
  const [cargando, setCargando] = useState(false)

  const actualizarCampo = (evento) => {
    const { name, value } = evento.target

    setActivo((datosAnteriores) => ({
      ...datosAnteriores,
      [name]: value,
    }))
  }

  const enviarFormulario = async (evento) => {
    evento.preventDefault()
    setMensaje('')

    if (activo.fechaCalculo < activo.fechaAdquisicion) {
      setMensaje(
        'La fecha de cálculo no puede ser anterior a la fecha de adquisición.',
      )
      return
    }

    setCargando(true)

    try {
      const solicitud = {
        nombre: activo.nombre.trim(),
        categoria: Number(activo.categoria),
        valorCompra: Number(activo.valorCompra),
        fechaAdquisicion: `${activo.fechaAdquisicion}T00:00:00`,
        fechaCalculo: `${activo.fechaCalculo}T00:00:00`,
      }

      const resultado = await calcularDepreciacion(solicitud)
      onCalculoRealizado(resultado)

      setActivo({
        ...activoInicial,
        fechaAdquisicion: fechaActual,
        fechaCalculo: fechaActual,
      })
    } catch (error) {
      setMensaje(error.message)
    } finally {
      setCargando(false)
    }
  }

  return (
    <section className="tarjeta-depreciacion">
      <div className="encabezado-seccion">
        <span className="etiqueta-seccion">Nuevo activo</span>
        <h2>Calcular depreciación</h2>
        <p>Ingresa la información contable del activo.</p>
      </div>

      <form className="formulario-activo" onSubmit={enviarFormulario}>
        <label htmlFor="activoNombre">Nombre del activo</label>
        <input
          id="activoNombre"
          name="nombre"
          type="text"
          value={activo.nombre}
          onChange={actualizarCampo}
          placeholder="Ejemplo: Computadora portátil"
          maxLength="150"
          required
        />

        <label htmlFor="activoCategoria">Categoría</label>
        <select
          id="activoCategoria"
          name="categoria"
          value={activo.categoria}
          onChange={actualizarCampo}
          required
        >
          {categorias.map((categoria) => (
            <option key={categoria.valor} value={categoria.valor}>
              {categoria.texto}
            </option>
          ))}
        </select>

        <label htmlFor="activoValor">Valor de compra</label>
        <input
          id="activoValor"
          name="valorCompra"
          type="number"
          value={activo.valorCompra}
          onChange={actualizarCampo}
          placeholder="0.00"
          min="0.01"
          step="0.01"
          required
        />

        <div className="fila-campos">
          <div>
            <label htmlFor="fechaAdquisicion">Fecha de adquisición</label>
            <input
              id="fechaAdquisicion"
              name="fechaAdquisicion"
              type="date"
              value={activo.fechaAdquisicion}
              onChange={actualizarCampo}
              required
            />
          </div>

          <div>
            <label htmlFor="fechaCalculo">Fecha de cálculo</label>
            <input
              id="fechaCalculo"
              name="fechaCalculo"
              type="date"
              value={activo.fechaCalculo}
              min={activo.fechaAdquisicion}
              onChange={actualizarCampo}
              required
            />
          </div>
        </div>

        {mensaje && <p className="mensaje error">{mensaje}</p>}

        <button
          type="submit"
          className="boton-principal"
          disabled={cargando}
        >
          {cargando ? 'Calculando...' : 'Calcular depreciación'}
        </button>
      </form>
    </section>
  )
}

export default FormularioActivo