import * as fs from 'fs';

const input = fs.readFileSync("input5.txt", "utf-8");
const inputDemo = `    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2`

class MoveInstruction {
    constructor(n: string, s: string, e: string) {
        this.numberToMove = parseInt(n);
        this.start = parseInt(s)-1;
        this.end = parseInt(e)-1;
    }
    numberToMove: number;
    start: number;
    end: number
    applyMove(crateStacks: string[][]) {
        for (let i = 0; i < this.numberToMove; i++) {
            const movingCrate = crateStacks[this.start].pop();
            if (movingCrate == undefined) {
                throw `No crates on ${this.start} crate stack`;
            }
            crateStacks[this.end].push(movingCrate);
        }
        return crateStacks;
    }
    applyMove2(crateStacks: string[][]) {
        const s = crateStacks[this.start].length - this.numberToMove;
        const movingCrates = crateStacks[this.start].splice(s);
        crateStacks[this.end].push(...movingCrates);
        return crateStacks;
    }
}

function parseMoves(input: string): MoveInstruction[] {
    const r = input
        .split('\n')
        .map(line => {
            line = line.replace('move ','')
                .replace('from ', '')
                .replace('to ', '');
            const nums = line.split(' ');
            return new MoveInstruction(nums[0], nums[1], nums[2])
        });
    return r;
}

function parseCrateStacks(input: string) : string[][] {
    const lines = input.split('\n');
    lines.pop();
    lines.reverse();

    let crates: string[][] = [];
    const crateStackCount = lines[0].length/4;
    for (let i = 0; i < crateStackCount; i++) {
        let crateStack: string[] = [];
        lines.forEach(line => {
            if (line[i*4+1] != ' ') {
                crateStack.push(line[i*4+1]);
            }
        });
        crates.push(crateStack);
    }
    return crates;
}

function readTopOfCrates(crates: string[][]) {
    return crates.reduce((acc,crate) => {
        return acc+crate[crate.length-1];
    }, '');
}

const sections = input.split("\n\n");
const cratesString = sections[0]
const movesString = sections[1]

let crateStacks1 = parseCrateStacks(cratesString);
let crateStacks2 = parseCrateStacks(cratesString);
const moves = parseMoves(movesString);

let part1 = 
    moves.reduce((stacks, move) => {
        return move.applyMove(stacks);
    }, crateStacks1);

let part2 =
    moves.reduce((stacks, move) => {
        return move.applyMove2(stacks);
    }, crateStacks2);

const result1 = readTopOfCrates(part1);
console.log(result1);

const result2 = readTopOfCrates(part2);
console.log(result2);