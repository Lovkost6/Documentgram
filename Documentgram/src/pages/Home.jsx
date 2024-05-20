import {useState,useEffect} from "react";

const tabs = {
    sending: 1,
    recipients:2,
    home:3
}
export const Home = ({setAuth,auth}) => {
    const [message, setMessage] = useState([])
    const [tab, setTab] = useState(tabs.home)
    useEffect(() => {
        sentMessages()
    }, []);
    const sentMessages = async () => {
        const responseMessages = await fetch("https://localhost:44345/sent-messages", {headers : {"Authuserid": auth}})
            .then(res => res.json())
        setMessage(responseMessages)
    }
    
    const deleteMessage = async (id) => {
        const deleteResponse = await fetch(`https://localhost:44345/v1/documents/${id}`,{method:"DELETE",headers : {"Authuserid": auth}})
        sentMessages()
    }
    return(
        <div>
            
        <button onClick={e => setTab(tabs.recipients)}>Полученные</button>
        <button onClick={e => setTab(tabs.sending)}>Отправленные</button>
        <button onClick={e => setAuth(null)}>Выйти</button>
            {
                tab == tabs.sending ? <div>
                    {message.map(x =>
                        <div>
                            <div>Тема: {x.name}</div>
                            <div>Сообщение: {x.description}</div>
                            <div>Вложения: {x.picturePath}</div>
                            <div>Получатели:</div>
                            {
                                x.names.map(k => <div>{k}</div>)
                            }
                            <button onClick={e => deleteMessage(x.id)}>Удалить</button>
                            <div>++++++++++++++++++</div>
                        </div>
                    )}
                </div> : null
            }

        </div>
    )
}