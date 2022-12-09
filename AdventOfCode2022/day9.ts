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

type Move =
    | "U"
    | "L"
    | "R"
    | "D"

class Pos {
    x: number = 0
    y: number = 0
    toString() {
       return `${this.x}-${this.y}`
    }
}

class Rope {
    attached: Rope | null = null
    pos = new Pos()
    positions: string[] = []
    recordPos() {
        this.positions.push(this.pos.toString());
    }
    move(move: Move) {
        this.recordPos()
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
    pullAttached() {
        if (this.attached != null) {
            this.attached.recordPos();
            if (this.pos.x > this.attached.pos.x + 1) {
                this.attached.pos.x = this.pos.x - 1;
                this.attached.pos.y = this.pos.y;
            }
            else if (this.pos.x < this.attached.pos.x - 1) {
                this.attached.pos.x = this.pos.x + 1;
                this.attached.pos.y = this.pos.y;
            }
            else if (this.pos.y > this.attached.pos.y + 1) {
                this.attached.pos.y = this.pos.y - 1;
                this.attached.pos.x = this.pos.x;
            }
            else if (this.pos.y < this.attached.pos.y - 1) {
                this.attached.pos.y = this.pos.y + 1;
                this.attached.pos.x = this.pos.x;
            }
        }
    }
}

const head = new Rope()
const tail = new Rope()
head.attached = tail;

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

let moves = parseInput(input);

moves.forEach(move => {
    head.move(move);
    console.log(move, head.pos.toString(), tail.pos.toString());
});
console.log("lines",input.split('\n').length)
console.log("moves",moves.length)
console.log((new Set(tail.positions)).size)
//console.log((new Set(tail.positions)))