import {useEffect, useState} from "react";


export const Recipient = ({auth}) => {
    const [message, setMessage] = useState([])
    useEffect(() => {
        recipientMessages()
    }, []);
    const recipientMessages = async () => {
        const responseMessages = await fetch("https://localhost:44345/v1/documents/recipient-messages", {headers: {"Authuserid": auth}})
            .then(res => res.json())
        setMessage(responseMessages)
    }
    const changeState = async (messageId,stateId) => {
        const state = await fetch("https://localhost:44345/v1/documents",{headers: {"Authuserid": auth,"Content-Type": "application/json"},method : "PATCH", body: JSON.stringify({Id: messageId,State:stateId})})
        console.log(messageId,stateId)
    }
    return (
        <div class="message-form">
            {message.map((x,id) =>
                <div className="message-item" key={id}>
                    <div className="form-group"><b>Тема:&nbsp; </b> {x.name}</div>
                    <div className="form-group"><b>Сообщение:&nbsp; </b> {x.description} </div>
                    <div className="form-group"><b>Вложения:&nbsp;</b></div>
                    <img src={x.file}/>
                    <div className="form-group"><b>От кого:&nbsp;</b> {x.owner}</div>
                    <div className="form-group"><b>Получатели:&nbsp;</b>
                        {
                            x.names.map(k => <div key={k}>{k}&nbsp;</div>)
                        }</div>
                    <button onClick={e => changeState(x.messageId,1)}>Согласовать</button>
                    <button onClick={e => changeState(x.messageId,2)}>Отклонить</button>
                </div>
            )}
        </div>
    )
}