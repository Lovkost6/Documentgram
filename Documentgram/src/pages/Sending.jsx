import {useEffect, useState} from "react";

export const Sending = ({auth}) => {
    const [message, setMessage] = useState([])
    useEffect(() => {
        sentMessages()
    }, []);
    const sentMessages = async () => {
        const responseMessages = await fetch("https://localhost:44345/v1/documents/sent-messages", {headers: {"Authuserid": auth}})
            .then(res => res.json())
        setMessage(responseMessages)
    }

    const deleteMessage = async (id) => {
        const deleteResponse = await fetch(`https://localhost:44345/v1/documents/${id}`, {
            method: "DELETE",
            headers: {"Authuserid": auth}
        })
        sentMessages()
    }

    return (
        <div class="message-form">
            {message.map(x =>
                <div className="message-item" key={x.id}>
                    <div  className="form-group"><b>Тема:&nbsp;</b> {x.name}</div>
                    <div className="form-group"><b>Сообщение:&nbsp;</b> {x.description}</div>
                    <div className="form-group"><b>Вложения:&nbsp;</b> {x.picturePath}</div>
                    <div className="form-group"><b>Получатели:&nbsp;</b>
                    {
                        x.names.map(k => <div key={k}>{k}&nbsp;</div>)
                    }</div>
                    <button onClick={e => deleteMessage(x.id)}>Удалить</button>
                </div>
            )}
        </div>
    )
}