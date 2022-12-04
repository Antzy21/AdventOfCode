import * as fs from 'fs';

const input = fs.readFileSync("input4.txt", "utf-8");
const inputDemo = `2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8`

function parseInput(input: string): ElfSection[][] {
    const lines = input.split('\n')
    return lines.map(line => {
        return line.split(',').map(elfSection => {
            return new ElfSection(elfSection);
        })
    });
}

class ElfSection {
    constructor(section: string) {
        //console.log(section)
        const startAndEnd = section.split('-')
        this.start = parseInt(startAndEnd[0]);
        this.end = parseInt(startAndEnd[1]);
    }
    start: number
    end: number
    containsFellowElfsSection(feSec: ElfSection) {
        return (this.start <= feSec.start && this.end >= feSec.end)
    }
    overlapsWithFellowElf(feSec: ElfSection) {
        return (this.start <= feSec.end && this.end >= feSec.start)
    }
}

const result =
    parseInput(input)
    .reduce((acc, elfPair) => {
        if (elfPair[0].containsFellowElfsSection(elfPair[1])){
            return acc + 1;
        }
        else if (elfPair[1].containsFellowElfsSection(elfPair[0])){
            return acc + 1;
        }
        return acc
    }, 0);
    
console.log(result);

const result2 =
    parseInput(input)
    .reduce((acc, elfPair) => {
        if (elfPair[0].overlapsWithFellowElf(elfPair[1])){
            return acc + 1;
        }
        return acc
    }, 0);

console.log(result2);