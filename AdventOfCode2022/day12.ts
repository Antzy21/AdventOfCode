import * as fs from 'fs';

const input = fs.readFileSync("input12.txt", "utf-8").trim();
const inputDemo = `Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi`

class heightMap {
    constructor(input: string) {
        this.map = input.split('\n').map((line, i) => {
            return line.split('').map((char, j) => {
                if (char === "S") {
                    this.start = [i,j]
                } else if (char === "E") {
                    this.end = [i,j]
                }
                const p = new point(char, i, j);
                return p;
            })
        });
    }
    map: point[][]
    start: [number, number]
    end: [number, number]
    print(prop: (p: point) => any) {
        this.map.forEach(plane => {
            console.log(plane.reduce((line, p) => {
                if (`${prop(p)}`.length == 1) {
                    line += `  ${prop(p)}`
                } else if (`${prop(p)}`.length == 2) {
                    line += ` ${prop(p)}`
                } else {
                    line += `${prop(p)}`
                }
                return line;
            }, ""));
        });
    }
    getTraversablePoints(p: point, travRule: (adjP: point, p: point) => boolean) {
        let travPoints: point[] = []
        if (p.i > 0) {
            let adjP = this.map[p.i-1][p.j]
            if (travRule(p, adjP)) {
                travPoints.push(adjP);
            }
        }
        if (p.i < this.map.length - 1) {
            let adjP = this.map[p.i+1][p.j]
            if (travRule(p, adjP)) {
                travPoints.push(adjP);
            }
        }
        if (p.j > 0) {
            let adjP = this.map[p.i][p.j-1]
            if (travRule(p, adjP)) {
                travPoints.push(adjP);
            }
        }
        if (p.j < this.map[0].length - 1) {
            let adjP = this.map[p.i][p.j+1]
            if (travRule(p, adjP)) {
                travPoints.push(adjP);
            }
        }
        return travPoints;
    }
    traverseStart() {
        const start = this.map[this.start[0]][this.start[1]]
        start.distanceFromOrigin = 0;
        const rule = (newDist: number, dist: number): boolean => newDist+1 < dist
        const travRule = (p:point, adjP: point): boolean => (adjP.height -1 <= p.height)
        this.traverse(start, travRule, rule)
    }
    traverseEnd() {
        const start = this.map[this.end[0]][this.end[1]]
        start.distanceFromOrigin = 0;
        const rule = (newDist: number, dist: number): boolean => newDist+1 < dist
        const travRule = (p:point, adjP: point): boolean => (adjP.height +1 >= p.height)
        this.traverse(start, travRule, rule)
    }
    traverse(p: point, travRule: (adjP: point, p: point) => boolean, rule: (newDist: number, dist: number) => boolean) {
        //heightmap.printDist();
        let updated = this.getTraversablePoints(p, travRule).filter(point => point.updated(rule, p.distanceFromOrigin))
        updated.forEach(point => {
            this.traverse(point, travRule, rule);
        });
    }
    getEndDistance() {
        return this.map[this.end[0]][this.end[1]].distanceFromOrigin
    }
    getClosestFlatPoint() {
        return this.map.flat()
            .filter((p: point) => p.height == 0 && !Number.isNaN(p.distanceFromOrigin))
            .map(p => p.distanceFromOrigin)
            .sort((p1, p2) => p1 - p2)
            [0]
    }
    printDist() {
        this.print((p: point) => {
            if (p.distanceFromOrigin > 100) {
                return ".."
            }
            return p.distanceFromOrigin
        })
    }
}

class point {
    constructor(char: string, i: number, j: number) {
        this.i = i;
        this.j = j;
        if (char == "S") {
            this.height = 0;
        } else if (char == "E") {
            this.height = 25;
        } else {
            this.height = char.charCodeAt(0)-97;
        }
    }
    i: number;
    j: number;
    height: number
    distanceFromOrigin: number = NaN;
    updated(rule: (newDist: number, dist: number) => boolean, newDist: number) {
        if (Number.isNaN(this.distanceFromOrigin) || rule(newDist, this.distanceFromOrigin)) {
            this.distanceFromOrigin = newDist+1;
            return true;
        }
        return false;
    }
}

const heightmap = new heightMap(input);

//heightmap.traverseStart();
heightmap.traverseEnd();

console.log(heightmap.getEndDistance())

//heightmap.printDist();
console.log("")
heightmap.print(p => {
    if (p.height == 0) {
        return p.distanceFromOrigin;
    }
    else {
        return ".."
    }
});

console.log(heightmap.getClosestFlatPoint());
