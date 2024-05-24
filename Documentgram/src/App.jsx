import { useState } from 'react'
import './App.css'
import {SignIn} from './pages/SignIn.jsx'
import {Home} from './pages/Home.jsx'
import { useLocalStorage } from './pages/Utils.jsx';
import Cookies from 'js-cookie'
function App() {
  const [auth, setAuth] = useState(false)
  return (
      auth == false ? <SignIn setAuth = {setAuth}/> : <Home removeAuth = {setAuth}/>
  )
}

export default App
