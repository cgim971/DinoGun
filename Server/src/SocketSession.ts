import WebSocket, { RawData } from "ws";
import { dinoGunio } from "./packet/packet";
import PacketManager from "./PacketManager";

export default class SocketSession {
    socket: WebSocket;
    playerId: number;

    position: dinoGunio.Position = new dinoGunio.Position({ x: 0, y: 0, scaleX: 1, gunRotate: 0 });

    constructor(socket: WebSocket, playerId: number, CloseCallback: Function) {
        this.socket = socket;
        this.playerId = playerId;

        this.socket.on("close", () => {
            console.log(`플레이어 ${this.playerId} 종료하였습니다.`);
            CloseCallback();
        });
    }

    getInt16FEFromBuffer(buffer: Buffer): number {
        return buffer.readInt16LE();
    }

    receiveMsg(data: RawData): void {
        let code: number = this.getInt16FEFromBuffer(data.slice(2, 4) as Buffer);
        PacketManager.Instance.handlerMap[code].handleMsg(this, data.slice(4) as Buffer);
    }

    sendData(payload: Uint8Array, msgCode: number): void {
        let len: number = payload.length + 4;

        let lenBuffer: Uint8Array = new Uint8Array(2);
        new DataView(lenBuffer.buffer).setUint16(0, len, true);

        let msgCodeBuffer: Uint8Array = new Uint8Array(2);
        new DataView(msgCodeBuffer.buffer).setUint16(0, msgCode, true);

        let sendBuffer: Uint8Array = new Uint8Array(len);
        sendBuffer.set(lenBuffer, 0);
        sendBuffer.set(msgCodeBuffer, 2);
        sendBuffer.set(payload, 4);

        this.socket.send(sendBuffer);
    }

}