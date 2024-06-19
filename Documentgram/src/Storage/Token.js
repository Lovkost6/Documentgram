import { createEvent, createStore,createEffect } from 'effector';
export const setToken = createEvent();
const unwatchCookiesSet = setToken.watch((token) => localStorage.setItem("token",JSON.stringify(token)));

export const resetToken = createEvent()
const unwatchCookiesRemove = resetToken.watch(() => {
    localStorage.removeItem("token")
})

const readLocalStorage = createEffect(
    () => {
        const user = localStorage.getItem("token")
        if (user == null){
            return null
        } else{
            return JSON.parse(user)
        }
    })
export const $token = createStore(null)
    .on(readLocalStorage.doneData,(_, payload) => payload)
    .on(setToken, (_, payload) => payload).reset(resetToken);

readLocalStorage()

