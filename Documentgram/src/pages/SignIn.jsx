import {useState} from "react";
import Cookies from 'js-cookie'
export const SignIn = ({setAuth}) => {
    const [login, setLogin] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    async function  getAuthData(event){
        event.preventDefault()
        const response = await fetch(`https://localhost:44345/v1/auth?login=${login}&password=${password}`,{method: "GET" ,withCredentials: true,
            credentials: 'include', headers : {'Access-Control-Allow-Origin': '*'}})
        response.status == 200? setAuth(true) : setError("Нет такого пользователя")
    }
    
    return(
        <form onSubmit={getAuthData}>
            <div>
                <label>Логин</label>
                <input value={login} onChange={event => setLogin(event.target.value)}/>
            </div>
            <div>
                <label>Пароль</label>
                <input value={password} onChange={event => setPassword(event.target.value)} />
            </div>
            <div>
                <label>{error}</label>
            </div>
            <button>Вход</button>
        </form>
    )
}