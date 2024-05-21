import {useState, useEffect} from "react";
import {Sending} from './Sending.jsx'
import {Recipient} from './Recipient.jsx'
import { useLocalStorage } from './Utils.jsx';
import {Create} from './Create.jsx';


    const tabs = {
        sending: 1,
        recipients: 2,
        home: 3,
        create: 4
    }
export const Home = ({setAuth, auth}) => {
    const [tab, setTab] = useLocalStorage("tab",tabs.home)
    

    return (
        <div>

            <button onClick={e => setTab(tabs.recipients)}>Полученные</button>
            <button onClick={e => setTab(tabs.sending)}>Отправленные</button>
            <button onClick={e => setTab(tabs.create)}>Создать сообщение</button>
            <button onClick={e => setAuth(null)}>Выйти</button>
            <div>{tab == 1 ? <Sending auth={auth}/> : tab == 2 ? <Recipient auth={auth}/> : tab == 4 ? <Create auth={auth}/> : <div>Home</div>}</div> 
        </div>
    )
}