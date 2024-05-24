import {useState, useEffect} from "react";
import {Sending} from './Sending.jsx'
import {Recipient} from './Recipient.jsx'
import { useLocalStorage } from './Utils.jsx';
import {Create} from './Create.jsx';
import Cookies from 'js-cookie'


    const tabs = {
        sending: 1,
        recipients: 2,
        home: 3,
        create: 4
    }
export const Home = ({removeAuth}) => {
    const [tab, setTab] = useLocalStorage("tab",tabs.home)
    
const  logout = async () =>{
    const responseMessages = await fetch("https://localhost:44345/v1/auth/logout", {withCredentials: true,
        credentials: 'include'})
    removeAuth(false)
}
    
    return (
        <div className="container">
            <nav className="navbar">
                <button className="nav-button" onClick={e => setTab(tabs.recipients)}>Полученные</button>
                <button className="nav-button" onClick={e => setTab(tabs.sending)}>Отправленные</button>
                <button className="nav-button" onClick={e => setTab(tabs.create)}>Создать сообщение</button>
            </nav>
                <button className="nav-button logout-button" onClick={e => logout()}>Выйти</button>
                <div>{tab == 1 ? <Sending/> : tab == 2 ? <Recipient/> : tab == 4 ?
                    <Create/> : <div>Home</div>}</div>
        </div>
)
}