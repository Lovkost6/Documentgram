import { useState, useEffect} from "react";
import {$token} from "../Storage/Token.js";
import {useUnit} from "effector-react";

export const Create = ({auth}) => {
    const [message, setMessage] = useState({name:"",description:"",picturePath:"", recipientsId:[]})
    const [users, setUsers] = useState([])
    const token = useUnit($token)
    const recipientMessages = async (e) => {
        e.preventDefault()
        const responseMessages = await fetch("https://localhost:44345/v1/documents", {method: "POST",headers: {"Authuserid": auth,"Content-Type": "application/json","Authorization" : `Bearer ${token}`}, body: JSON.stringify(message)})
        console.log(responseMessages)
          //  .then(res => res.json())
        //setMessage(responseMessages)
    }
    useEffect(() => {
        getAllUser()
    }, []);
    
    const getAllUser = async () => {
        const users = await  fetch("https://localhost:44345/v1/users" , {headers: {"Authuserid": auth,"Authorization" : `Bearer ${token}`}})
            .then(res => res.json())
        setUsers(users)
    }

    return (
        <div class="message-form">
            <form onSubmit={recipientMessages}>
                <div className="form-group">
                    <label>Тема:</label>
                    <input value={message.name}
                           onChange={event => setMessage(e => ({...e, name: event.target.value}))}/>
                </div>
                <div className="form-group">
                    <label>Сообщение:</label>
                    <input value={message.description}
                           onChange={event => setMessage(e => ({...e, description: event.target.value}))}/>
                </div>
                <div className="form-group">
                    <label>Вложения:</label>
                    <input value={message?.picturePath}
                           onChange={event => setMessage(e => ({...e, picturePath: event.target.value}))}/>
                </div>
                <div className="form-group">
                    <label>Получатели:</label>
                    <select multiple={true} onChange={event => {
                        var options = event.target.options;
                        var value = [];
                        for (var i = 0, l = options.length; i < l; i++) {
                            if (options[i].selected) {
                                value.push(options[i].value);
                            }
                        }
                        setMessage(e => ({...e, recipientsId: value}))
                    }}>{users.map(e => <option key={e.id} value={e.id}>{e.name}</option>)} </select>
                </div>
                    <button>Отправить</button>
            </form>
        </div>
)
}