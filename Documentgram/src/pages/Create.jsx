﻿import { useState, useEffect} from "react";

export const Create = ({auth}) => {
    const [message, setMessage] = useState({name:"",description:"",picturePath:"", recipientsId:[]})
    const [users, setUsers] = useState([])
    const recipientMessages = async (e) => {
        e.preventDefault()
        const responseMessages = await fetch("https://localhost:44345/v1/documents", {method: "POST",headers: {"Authuserid": auth,"Content-Type": "application/json"}, body: JSON.stringify(message)})
        console.log(responseMessages)
          //  .then(res => res.json())
        //setMessage(responseMessages)
    }
    useEffect(() => {
        getAllUser()
    }, []);
    
    const getAllUser = async () => {
        const users = await  fetch("https://localhost:44345/v1/users" , {headers: {"Authuserid": auth}})
            .then(res => res.json())
        setUsers(users)
    }

    return (
        <form onSubmit={recipientMessages}>
                <div>Тема:</div>
                <input value={message.name} onChange={event => setMessage(e => ({...e, name : event.target.value}))}/>
                
                <div>Сообщение:</div>
                <input value={message.description} onChange={event => setMessage(e => ({...e, description : event.target.value}))}/>
                
                <div>Вложения:</div>
                <input value={message?.picturePath} onChange={event => setMessage(e => ({...e, picturePath:event.target.value}))}/>
                
                <div>Получатели:</div>
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
                <button>Отправить</button>
        </form>
    )
}