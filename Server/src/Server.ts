import Express, { Application } from 'express'
import { IncomingMessage } from 'http';
import WS, { RawData } from 'ws';

const App : Application = Express();

const httpServer = App.listen(50000, ()=>{
    console.log("Server is running on 50000 port");
});

const socketServer :WS.Server = new WS.Server({
    server:httpServer,
}, ()=>{
    console.log("Socket server is running on 50000 port");
});

socketServer.on("connection", (soc:WS, req:IncomingMessage)=>{

    soc.on("message", (data:RawData, isBinary:boolean)=>{
    });

});