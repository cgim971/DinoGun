import SocketSession from "../SocketSession";
import { dinoGunio } from "./packet";
import { PacketHandler } from "./PacketHandler";

export default class CMoveHandler implements PacketHandler{
    handleMsg(session: SocketSession, buffer: Buffer): void {
        let cMove= dinoGunio.C_Move.deserialize(buffer);
        
        session.position = cMove.position;
    }
}