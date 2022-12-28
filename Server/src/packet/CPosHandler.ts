import SocketSession from "../SocketSession";
import { dinoGunio } from "./packet";
import { PacketHandler } from "./PacketHandler";

export default class CPosHandler implements PacketHandler{
    handleMsg(session: SocketSession, buffer: Buffer): void {
        let cPos= dinoGunio.C_Pos.deserialize(buffer);
        
        console.log(cPos.x);
    }
}