import SocketSession from "./SocketSession";

interface SessionDictionary {
    [key: number]: SocketSession;
}

export default class SessionManager {
    static Instance: SessionManager;
    sessionMap: SessionDictionary;

    constructor() {
        this.sessionMap = {};
    }

    addSession(session: SocketSession, id: number) {
        this.sessionMap[id] = session;
    }

    removeSession(id: number) {
        delete this.sessionMap[id];
    }

    broadCastMessage(payload: Uint8Array, msgCode: number, senderId: number = 0, exceptSender: boolean = false): void {
        for (let index in this.sessionMap) {
            if (exceptSender == true && senderId == this.sessionMap[index].playerId)
                continue;

            this.sessionMap[index].sendData(payload, msgCode);
        }
    }
}