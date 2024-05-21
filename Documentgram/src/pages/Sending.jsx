import {useEffect, useState} from "react";

export const Sending = ({auth}) => {
    const [message, setMessage] = useState([])
    useEffect(() => {
        sentMessages()
    }, []);
    const sentMessages = async () => {
        const responseMessages = await fetch("https://localhost:44345/sent-messages", {headers: {"Authuserid": auth}})
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
        <div>
            {message.map(x =>
                <div key={x.id}>
                    <div>Тема: {x.name}</div>
                    <div>Сообщение: {x.description}</div>
                    <div>Вложения: {x.picturePath}</div>
                    <div>Получатели:</div>
                    {
                        x.names.map(k => <div key={k}>{k}</div>)
                    }
                    <button onClick={e => deleteMessage(x.id)}>Удалить</button>
                    <div>++++++++++++++++++</div>
                </div>
            )}
        </div>
    )
}