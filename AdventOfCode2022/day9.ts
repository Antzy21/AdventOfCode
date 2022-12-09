import { dir } from 'console';
import * as fs from 'fs';
import { parse } from 'path';

const input = fs.readFileSync("input9.txt", "utf-8").trim();
const inputDemo = `R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2`
const inputDemo2 = `R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20`

type Move =
    | "U"
    | "L"
    | "R"
    | "D"

type Coordinates = {x: number; y: number}

class Rope {
    attached: Rope | null = null;
    pos : Coordinates = {x: 0, y: 0};
    positions: Coordinates[] = [this.pos];
    maxHeight() {
        return this.positions.reduce((acc, h) => {
            return Math.max(acc, h.y)
        }, 0)
    }
    minHeight() {
        return this.positions.reduce((acc, h) => {
            return Math.min(acc, h.y)
        }, 0)
    }
    maxWide() {
        return this.positions.reduce((acc, h) => {
            return Math.max(acc, h.x)
        }, 0)
    }
    minWide() {
        return this.positions.reduce((acc, h) => {
            return Math.min(acc, h.x)
        }, 0)
    }
    move(move: Move) {
        this.positions.push(this.pos);
        switch (move) {
            case "D":
                this.pos.y += -1;
                break;
            case "U":
                this.pos.y += 1;
                break;
            case "L":
                this.pos.x += -1;
                break;
            case "R":
                this.pos.x += 1;
                break;
        }
        this.pullAttached()
    }
    moveTo(x: number, y: number) {
        this.pos = {x, y};
        this.positions.push(this.pos);
        this.pullAttached();
    }
    moveBy(dx: number, dy: number) {
        this.pos = {x: this.pos.x+dx, y: this.pos.y+dy};
        this.positions.push(this.pos);
        this.pullAttached();
    }
    pullAttached() {
        if (this.attached != null) {
            let dy = this.pos.y - this.attached.pos.y
            let dx = this.pos.x - this.attached.pos.x
            if (Math.abs(dx) < 2 && Math.abs(dy) < 2) {
            }
            else {
                
                this.attached.moveBy(Math.round(dx/2+dx/1000), Math.round(dy/2+dy/1000));
            }
        }
    }
}

function parseInput(input: string) {
    let moves: Move[] = []
    input.split('\n').forEach(line => {
        let move = line[0] as Move;
        let dist = parseInt(line.substring(1).trim())
        for (let i = 0; i < dist; i++) {
            moves.push(move);
        }
    })
    return moves;
}

function isUnique(c: Coordinates, i: number, array: Coordinates[]) {
    return array.findIndex((v, j) => {
        return (v.x == c.x && v.y == c.y && i > j)
    }) == -1
}

function print(ropes: Rope[]) {
    for (let j = 5; j >= 0; j--) {
        let line = ""
        for (let i = 0; i <= 5; i++) {
            line += ropes.reduceRight((acc, rope, v) => {
                if (rope.pos.x == i && rope.pos.y == j) {
                    if (v == 0) {
                        acc = "H";
                    } else {acc = `${v}`}
                }
                return acc;
            }, '.')
        }
        console.log(line);
    }
}

function printEnd(rope: Rope) {
    for (let j = rope.maxHeight(); j >= rope.minHeight(); j--) {
        let line = "";
        for (let i = rope.minWide(); i <= rope.maxWide(); i++) {
            line += rope.positions.reduce((acc, pos) => {
                if (pos.x == i && pos.y == j) {
                    acc = "#";
                }
                return acc;
            }, '.');
        }
        console.log(line);
    }
}

function solve(input: string, ropeLength: number) {
    const rope: Rope[] = [new Rope()];
    for (let r = 1; r < ropeLength; r++) {
        let r = new Rope();
        rope[rope.length-1].attached = r
        rope.push(r)
    }

    print(rope);
    let moves = parseInput(input);
    moves.forEach(move => {
        rope[0].move(move)
        //console.log(move);
        //print(rope);
    });
    const tail = rope[rope.length-1]

    console.log("lines",input.split('\n').length)
    console.log("moves",moves.length)
    const uniquePositions = tail.positions.filter(isUnique)
    //console.log(tail.positions)
    //console.log(uniquePositions)
    console.log(tail.positions.length);
    console.log(uniquePositions.length);

    //printEnd(tail);
}

//solve(inputDemo, 10);
solve(input, 10);