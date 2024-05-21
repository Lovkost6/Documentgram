import {useEffect, useState} from "react";

export const Recipient = ({auth}) => {
    const [message, setMessage] = useState([])
    useEffect(() => {
        recipientMessages()
    }, []);
    const recipientMessages = async () => {
        const responseMessages = await fetch("https://localhost:44345/v1/documents", {headers: {"Authuserid": auth}})
            .then(res => res.json())
        setMessage(responseMessages)
    }
    return (
        <div>
            {message.map((x,id) =>
                <div key={id}>
                    <div>Тема: {x.name}</div>
                    <div>Сообщение: {x.description}</div>
                    <div>Вложения: {x.picturePath}</div>
                    <div>От кого: {x.owner}</div>
                    <div>Получатели:</div>
                    {
                        x.names.map(k => <div key={k}>{k}</div>)
                    }
                    <div>++++++++++++++++++</div>
                </div>
            )}
        </div>
    )
}