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

        if (code == dinoGunio.MSGID.C_MOVE) {
            let cPos: dinoGunio.C_Move = dinoGunio.C_Move.deserialize(payload);
            console.log(cPos.plyaerId);
            console.log(cPos.spawnPosition.x);
            console.log(cPos.spawnPosition.y);
        }

    });
});