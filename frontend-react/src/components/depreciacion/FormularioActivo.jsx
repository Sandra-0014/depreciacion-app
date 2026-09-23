import { useState } from 'react'

const formularioInicial = {
  nombre: '',
  categoria: '',
  valorCompra: '',
  fechaAdquisicion: '',
  fechaCalculo: '',
}

const categorias = [
  {
    valor: 1,
    nombre: 'Equipos de cómputo y software',
    vidaUtil: '3 años',
  },
  {
    valor: 2,
    nombre: 'Instalaciones, maquinaria y muebles',
    vidaUtil: '10 años',
  },
  {
    valor: 3,
    nombre: 'Vehículos de transporte',
    vidaUtil: '5 años',
  },
  {
    valor: 4,
    nombre: 'Inmuebles y naves',
    vidaUtil: '20 años',
  },
]

function FormularioActivo({ onCalcular, cargando }) {
  const [formulario, setFormulario] = useState(formularioInicial)
  const [error, setError] = useState('')

  const actualizarCampo = (evento) => {
    const { name, value } = evento.target

    setFormulario((datosAnteriores) => ({
      ...datosAnteriores,
      [name]: value,
    }))

    setError('')
  }

  const enviarFormulario = async (evento) => {
    evento.preventDefault()
    setError('')

    if (!formulario.nombre.trim()) {
      setError('Ingresa el nombre del activo.')
      return
    }

    if (!formulario.categoria) {
      setError('Selecciona una categoría.')
      return
    }

    if (Number(formulario.valorCompra) <= 0) {
      setError('El valor de compra debe ser mayor que cero.')
      return
    }

    if (formulario.fechaCalculo <= formulario.fechaAdquisicion) {
      setError(
        'La fecha de cálculo debe ser posterior a la fecha de adquisición.',
      )
      return
    }

    const datosActivo = {
      nombre: formulario.nombre.trim(),
      categoria: Number(formulario.categoria),
      valorCompra: Number(formulario.valorCompra),
      fechaAdquisicion: formulario.fechaAdquisicion,
      fechaCalculo: formulario.fechaCalculo,
    }

    const calculadoCorrectamente = await onCalcular(datosActivo)

    if (calculadoCorrectamente) {
      setFormulario(formularioInicial)
    }
  }

  return (
    <section className="tarjeta-depreciacion formulario-activo">
      <div className="encabezado-seccion">
        <h2>Registro de activo</h2>
      </div>

      <form onSubmit={enviarFormulario}>
        <label htmlFor="nombreActivo">Nombre del activo</label>
        <input
          id="nombreActivo"
          name="nombre"
          type="text"
          value={formulario.nombre}
          onChange={actualizarCampo}
          placeholder="Ejemplo: Computadora de oficina"
          maxLength="150"
          required
        />

        <label htmlFor="categoriaActivo">Categoría</label>
        <select
          id="categoriaActivo"
          name="categoria"
          value={formulario.categoria}
          onChange={actualizarCampo}
          required
        >
          <option value="">Selecciona una categoría</option>

          {categorias.map((categoria) => (
            <option key={categoria.valor} value={categoria.valor}>
              {categoria.nombre} — {categoria.vidaUtil}
            </option>
          ))}
        </select>

        <label htmlFor="valorCompra">Valor de compra</label>
        <div className="campo-dinero">
          <span>$</span>
          <input
            id="valorCompra"
            name="valorCompra"
            type="number"
            value={formulario.valorCompra}
            onChange={actualizarCampo}
            placeholder="0.00"
            min="0.01"
            step="0.01"
            required
          />
        </div>

        <div className="fila-fechas">
          <div>
            <label htmlFor="fechaAdquisicion">Fecha de adquisición</label>
            <input
              id="fechaAdquisicion"
              name="fechaAdquisicion"
              type="date"
              value={formulario.fechaAdquisicion}
              onChange={actualizarCampo}
              required
            />
          </div>

          <div>
            <label htmlFor="fechaCalculo">Calcular hasta</label>
            <input
              id="fechaCalculo"
              name="fechaCalculo"
              type="date"
              value={formulario.fechaCalculo}
              onChange={actualizarCampo}
              min={formulario.fechaAdquisicion || undefined}
              required
            />
          </div>
        </div>

        <p className="nota-formulario">
          El sistema utilizará un valor residual fijo del 10%.
        </p>

        {error && <p className="mensaje error">{error}</p>}

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