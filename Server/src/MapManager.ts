import FS from 'fs';
import { dinoGunio } from './packet/packet'

export enum MapCategory {
    PATH = 0,
    BLOCK = 1,
    SPAWNPOINT = 2
}

export default class MapManager {
    static Instance: MapManager;

    mapData: number[][] = [];
    xMin: number = 0;
    xMax: number = 0;
    yMin: number = 0;
    yMax: number = 0;

    constructor(filePath: string) {
        let fileText: string = FS.readFileSync(filePath, "utf8");
        let line: string[] = fileText.split("\r\n");
        this.xMin = parseInt(line[0]);
        this.xMax = parseInt(line[1]);
        this.yMin = parseInt(line[2]);
        this.yMax = parseInt(line[3]);

        line = line.splice(4);
        let lineCount: number = Math.abs(this.yMin) + Math.abs(this.yMax);

        for (let i = 0; i < lineCount; i++) {
            let numberArr: number[] = line[i].split("").map(x => parseInt(x));
            this.mapData.push(numberArr);
        }

        this.mapData = this.mapData.reverse();
    }

    getMapData(x: number, y: number): MapCategory {
        x+= Math.abs(this.xMin);
        y+= Math.abs(this.yMin);
        
        return this.mapData[y][x];
    }
}