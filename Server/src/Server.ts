import Express, { Application } from 'express'
import { IncomingMessage } from 'http';
import WS, { RawData } from 'ws';
import MapManager from './MapManager';
import { dinoGunio } from './packet/packet';
import Path from 'path';

const App: Application = Express();

MapManager.Instance = new MapManager(Path.join(__dirname, "Tilemap.txt"));

const httpServer = App.listen(50000, () => {
    console.log("Server is running on 50000 port");
});

const socketServer: WS.Server = new WS.Server({
    server: httpServer,
}, () => {
    console.log("Socket server is running on 50000 port");
});

socketServer.on("connection", (soc: WS, req: IncomingMessage) => {

    soc.on("message", (data: RawData, isBinary: boolean) => {

        let length: number = (data.slice(0, 2) as Buffer).readInt16LE();
        let code: number = (data.slice(2, 4) as Buffer).readInt16LE();
        let payload: Buffer = data.slice(4) as Buffer;

        if (code == dinoGunio.MSGID.C_POS) {
            let cPos: dinoGunio.C_Pos = dinoGunio.C_Pos.deserialize(payload);
            console.log(cPos.x, cPos.y);
        }
    });
});