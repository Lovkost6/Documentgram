import {useEffect, useState} from "react";
const stateArr = ["Не рассмотрен","Согласован", "Отклонен"]
export const Sending = () => {
    const [message, setMessage] = useState([])
    useEffect(() => {
        sentMessages()
    }, []);
    const sentMessages = async () => {
        const responseMessages = await fetch("https://localhost:44345/v1/documents/sent-messages", {withCredentials: true,
            credentials: 'include'})
            .then(res => res.json())
        setMessage(responseMessages)
    }

    const deleteMessage = async (id) => {
        const deleteResponse = await fetch(`https://localhost:44345/v1/documents/${id}`, {
            method: "DELETE"
        })
        sentMessages()
    }

    return (
        <div className="message-form">
            {message.map(x =>
                <div className="message-item" key={x.id}>
                    <div  className="form-group"><b>Тема:&nbsp;</b> {x.name}</div>
                    <div className="form-group"><b>Сообщение:&nbsp;</b> {x.description}</div>
                    <div className="form-group"><b>Вложения:&nbsp;</b> </div>
                    <img  src={x.file}/>
                    <div className="form-group"><b>Получатели:&nbsp;</b>
                    {
                        x.names.map((k,id) => <div key={id}>{k.name + " " + stateArr[k.state]}&nbsp;</div>)
                    }</div>
                    <button onClick={e => deleteMessage(x.id)}>Удалить</button>
                </div>
            )}
        </div>
    )
}