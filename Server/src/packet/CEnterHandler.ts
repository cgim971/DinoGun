import SessionManager from "../SessionManager";
import SocketSession from "../SocketSession";
import { dinoGunio } from "./packet";
import { PacketHandler } from "./PacketHandler";

export default class CEnterHandler implements PacketHandler {
    handleMsg(session: SocketSession, buffer: Buffer): void {
        let cEnter = dinoGunio.C_Enter.deserialize(buffer);

        session.isEnter = true;
        // 새로 들어오면 포지션을 변경하고
        session.position = cEnter.position;

        // 정보를 생성 후
        let info = new dinoGunio.PlayerInfo({ playerId: session.playerId, position: session.position });
        // 넣는다.
        let sEnter = new dinoGunio.S_Enter({ playerInfo: info });

        // 이 내용을 모두에게 뿌려줌
        SessionManager.Instance.broadCastMessage(sEnter.serialize(), dinoGunio.MSGID.S_ENTER, session.playerId, true);

        let list = SessionManager.Instance.getPlayerList();
        let initMsg = new dinoGunio.S_InitList({playerList:list});
        session.sendData(initMsg.serialize(), dinoGunio.MSGID.S_INITLIST);
    }
}