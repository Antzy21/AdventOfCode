import * as fs from 'fs';

let input = fs.readFileSync("input3.txt", "utf-8");
let inputDemo = `vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw`;

class Rucksack {
    constructor(line: string) {
        this.contents = line
        const lineLen = line.length;
        this.compartment1 = line.slice(0, lineLen/2);
        this.compartment2 = line.slice(lineLen/2, lineLen);
    }
    contents: string
    compartment1: string;
    compartment2: string;
    findCommonItem(): string {
        for (let i = 0; i < this.compartment1.length; i++) {
            for (let j = 0; j < this.compartment1.length; j++) {
                if (this.compartment1[i] == this.compartment2[j]) {
                    return this.compartment1[i];
                }
            }
        }
        console.log(this.compartment1);
        console.log(this.compartment2);
        throw "no common Item";
    }
}

class Group {
    rucksacks: Rucksack[] = [];
    findCommonItem(): string {
        for (let i = 0; i < this.rucksacks[0].contents.length; i++) {
            let I = this.rucksacks[0].contents[i];
            for (let j = 0; j < this.rucksacks[1].contents.length; j++) {
                let J = this.rucksacks[1].contents[j];
                if (I == J) {
                    for (let k = 0; k < this.rucksacks[2].contents.length; k++) {
                        let K = this.rucksacks[2].contents[k]
                        if (J == K) {
                            return J;
                        }
                    }
                }
            }
        }
        throw "No Common Item"
    }
}

let alphabetValues = new Map<string, number> ([
    ["a", 1],
    ["b", 2],
    ["c", 3],
    ["d", 4],
    ["e", 5],
    ["f", 6],
    ["g", 7],
    ["h", 8],
    ["i", 9],
    ["j", 10],
    ["k", 11],
    ["l", 12],
    ["m", 13],
    ["n", 14],
    ["o", 15],
    ["p", 16],
    ["q", 17],
    ["r", 18],
    ["s", 19],
    ["t", 20],
    ["u", 21],
    ["v", 22],
    ["w", 23],
    ["x", 24],
    ["y", 25],
    ["z", 26]
])

function getItemValue(item: string) : number {
    let v = alphabetValues.get(item);
    if (v != undefined){
        return v;
    }
    const lowerItem = item.toLowerCase();
    v = alphabetValues.get(lowerItem);
    if (v != undefined) {
        return 26 + v;
    }
    throw "Error";
}

function parseRucksacks(input: string) : Rucksack[] {
    const lines = input.split('\n')
    let rucksacks: Rucksack[] = []
    lines.forEach(line => {
        const r = new Rucksack(line);
        rucksacks.push(r);
    });
    return rucksacks;
}

function parseGroups(input: string) : Group[] {
    const lines = input.split('\n')
    let groups: Group[] = []
    let g = new Group();
    let i = 1;
    lines.forEach(line => {
        g.rucksacks.push(new Rucksack(line));
        if (i % 3 == 0) {
            groups.push(g);
            g = new Group();
        }
        i ++;
    });
    return groups;
}

const rucksacks = parseRucksacks(input);
const result = 
    rucksacks
    .map((r)=> r.findCommonItem())
    .map((i) => getItemValue(i))
    .reduce((acc, v) => {
       return acc+v
    }, 0);

console.log(result)

const groups = parseGroups(input)
const result2 =
    groups
    .map((g)=> g.findCommonItem())
    //.forEach((v) => console.log(v))
    .map((i) => getItemValue(i))
    .reduce((acc, v) => {
    return acc+v
    }, 0);

    console.log(result2)   