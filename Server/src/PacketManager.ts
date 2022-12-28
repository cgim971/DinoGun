import { dinoGunio } from "./packet/packet";
import { PacketHandler } from "./packet/PacketHandler";
import CPosHandler from "./packet/CPosHandler";

interface HandlerDictionary {
    [key: number]: PacketHandler;
}

export default class PacketManager {
    static Instance: PacketManager;
    handlerMap: HandlerDictionary;

    constructor() {
        console.log("Packet Manager initialize...");
        this.handlerMap = {};
        this.register();
    }

    register(): void {
        this.handlerMap[dinoGunio.MSGID.C_POS] = new CPosHandler();
    }
}   