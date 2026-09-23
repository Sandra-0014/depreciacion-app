import { useState } from 'react'
import './App.css'
import './depreciacion.css'
import AuthPage from './pages/AuthPage'
import DepreciacionPage from './pages/DepreciacionPage'
import {
  cerrarSesion,
  obtenerUsuario,
} from './services/sessionService'

function App() {
  const [usuario, setUsuario] = useState(obtenerUsuario)

  const finalizarSesion = () => {
    cerrarSesion()
    setUsuario(null)
  }

  if (!usuario) {
    return <AuthPage onSesionIniciada={setUsuario} />
  }

  return (
    <DepreciacionPage
      usuario={usuario}
      onCerrarSesion={finalizarSesion}
    />
  )
}

export default App