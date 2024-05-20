import { useState } from 'react'
import './App.css'
import {SignIn} from './pages/SignIn.jsx'
import {Home} from './pages/Home.jsx'
function App() {
  const [auth, setAuth] = useState(null)
    
  return (
      auth == null? <SignIn setAuth = {setAuth}/> : <Home setAuth={setAuth} auth = {auth}/>
  )
}

export default App
