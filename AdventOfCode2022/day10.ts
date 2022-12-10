import * as fs from 'fs';

const input = fs.readFileSync("input10.txt", "utf-8").trim();
const inputDemo = `addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop`

class Cycle {
    value = 0;
    recordedScore: number[] = []
    recordedScreen: string = ""
    recordValue(x: number) {
        this.recordedScore.push(c.value*X)
    }
    recordScreen(X: number) {
        const pixel = this.getPixel(X);
        this.recordedScreen += pixel;
        if (this.value%40 == 39) {
            this.recordedScreen += '\n'
        }
    }
    getPixel(X:number){
        if (Math.abs(X-this.value%40)<2) {
            return '#'
        } else {
            return "."
        }
    }
    tick(X: number) {
        this.recordScreen(X);
        this.value ++
        if ((this.value-20)%40 == 0) {
            this.recordValue(X);
        }
    }
    total() {
        return this.recordedScore.reduce((acc, s) => acc+s, 0);
    }
}

type Instruction = "noop" | {addx : number};

function parseInput(input: string): Instruction[] {
    return input.split('\n').map(line => {
        line.trim()
        if (line == "noop") {
            return "noop" as Instruction
        }
        else {
            let x = parseInt(line.split(' ')[1])
            return {addx: x}
        }
    })
}

const instructions = parseInput(input);

let c = new Cycle();
let X = 1;

instructions.forEach(instruction => {
    if (instruction == "noop") {
        c.tick(X);
    }
    else {
        c.tick(X);
        c.tick(X);
        X += instruction.addx;
    }
});

console.log(c.total())

console.log(c.recordedScreen);