import * as fs from 'fs';
import { inspect } from 'util';

const input = fs.readFileSync("input11.txt", "utf-8").trim();
const inputDemo = `Monkey 0:
Starting items: 79, 98
Operation: new = old * 19
Test: divisible by 23
  If true: throw to monkey 2
  If false: throw to monkey 3

Monkey 1:
Starting items: 54, 65, 75, 74
Operation: new = old + 6
Test: divisible by 19
  If true: throw to monkey 2
  If false: throw to monkey 0

Monkey 2:
Starting items: 79, 60, 97
Operation: new = old * old
Test: divisible by 13
  If true: throw to monkey 1
  If false: throw to monkey 3

Monkey 3:
Starting items: 74
Operation: new = old + 3
Test: divisible by 17
  If true: throw to monkey 0
  If false: throw to monkey 1`

class Round {
    constructor(monkeys: Monkey[]) {
        this.monkeys = monkeys
        this.monkeys.forEach(monkey => {
            monkey.monkeyTrue = this.monkeys[monkey.testTrue]
            monkey.monkeyFalse = this.monkeys[monkey.testFalse]
        });
    }
    monkeys: Monkey[] = []
    print(roundCount: number) {
        console.log("\nAfter round", roundCount)
        this.monkeys.forEach((m, i) => {
            //console.log("Monkey", i, ":", m.print());
        })
        this.monkeys.forEach((m, i) => {
            console.log("Monkey", i, "inspected items",m.numberOfInspections,"times")
        });
    }
    run(handleWorrying: (n: number) => number, log = false) {
        this.monkeys.forEach((monkey, i) => {
            if (log) {console.log("Monkey", i,":")}
            monkey.handleItems(handleWorrying, log);
        });
    }
    calculateMonkeyBusiness() {
        let insPerMonkey = this.monkeys.map(m => m.numberOfInspections).sort((a,b)=>{return a-b});
        return insPerMonkey[insPerMonkey.length-1] * insPerMonkey[insPerMonkey.length-2]
    }
}

class Monkey {
    items: number[] = [];
    testTrue: string;
    testFalse: string;
    monkeyTrue: Monkey;
    monkeyFalse: Monkey;
    modTest: number;
    numberOfInspections: number = 0
    operation: (n : number) => number
    test: (n: number) => void
    handleItems(handleWorrying: (n: number) => number, log = false) {
        this.items.forEach(item => {
            if (log) {console.log("  Monkey inspects item with worry level", item)}
            let inspectedItem = this.inspect(item, handleWorrying, log);
            if (inspectedItem % this.modTest == 0) {
                if (log) {console.log("    Item with worry level", inspectedItem, "goes to Monkey", this.testTrue);}
                this.monkeyTrue.items.push(inspectedItem);
            }
            else {
                if (log) {console.log("    Item with worry level", inspectedItem, "goes to Monkey", this.testFalse);}
                this.monkeyFalse.items.push(inspectedItem);
            }
        });
        this.items = [];
    }
    inspect(item: number, handleWorrying: (n: number) => number, log=false) {
        this.numberOfInspections ++;
        let line = `${item}`;
        item = this.operation(item)
        if (log) {console.log("    Worry level goes from", line, "to", item)}
        item = handleWorrying(item)
        if (log) {console.log("    Worry level is divided by 3 to", item)}
        return item;
    }
    print() {
        return this.items.reduce((acc, i) => {
            return acc + `${i}, `;
        }, "Items: ")
        //if (log) {console.log("Test Result Directions:", "T:", this.testTrue, ", F:", this.testFalse)
        //if (log) {console.log("Op:", this.operation)
        //if (log) {console.log("Mod:", this.modTest)
    }
}

function parseInput(input: string) : Round {
    const monkeys = input.split('\n\n').map(monkeyString => {
        const lines = monkeyString.split('\n');
        const m = new Monkey();
        let itemStrings = lines[1].trim().replace("Starting items: ", "").split(', ');
        let items = itemStrings.map((i) => parseInt(i))
        m.items = items;
        const opStrings = lines[2].trim().replace("Operation: new = old ", "").split(' ')
        let op = (old: number) : number => {
            let otherNum = 0
            if (opStrings[1] == "old") {
                otherNum = old;
            }
            else {
                otherNum = parseInt(opStrings[1])
            }
            if (opStrings[0] == '*') {
                return old * otherNum;
            }
            if (opStrings[0] == '+') {
                return old + otherNum;
            }
            else {
                console.error(opStrings);
                throw `Can't handle operation ${opStrings[0]}`
            }
        }
        m.operation = op;
        m.testTrue = lines[4].split('')[lines[4].length -1]
        m.testFalse = lines[5].split('')[lines[5].length -1]
        m.modTest = parseInt(lines[3].split(' ')[lines[3].split(' ').length - 1])
        return m;
    });
    return new Round(monkeys);
}

function solve(numberOfRounds, round, handleWorrying) {
    for (let i = 1; i <= numberOfRounds; i++) {
        if (i % 1000 == 0 || i == numberOfRounds){
            //round.print(i)
        }
        round.run(handleWorrying)
    }
    console.log(round.calculateMonkeyBusiness());
}

const round = parseInput(input);
let handleWorrying1 = (item: number) => Math.floor(item/3)
const mod = round.monkeys.reduce((mod, monkey) => {
    return mod * monkey.modTest;
},1)
let handleWorrying2 = (item: number) => {
    return item % mod;
}

let numberOfRounds = 20
solve(numberOfRounds, round, handleWorrying1);

numberOfRounds = 10000
solve(numberOfRounds, round, handleWorrying2);
