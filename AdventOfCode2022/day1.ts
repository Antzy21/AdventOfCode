import * as fs from 'fs';

const input = fs.readFileSync("input1.txt", "utf-8");
const inputDemo = `1000
2000
3000

4000

5000
6000

7000
8000
9000

10000`

class elf {
    foods: number[] = [];
    totalFood() {
        return this.foods.reduce((acc, cur) =>{
            return acc + cur
        }, 0);
    }
}

const lines = input.split('\n')

let currentElf = new elf();
const elves : elf[] = [];

lines.forEach(line => {
    if (line == "") {
        elves.push(currentElf);
        currentElf = new elf();
    }
    else {
        let foodCount = parseInt(line);
        currentElf.foods.push(foodCount);
    }
});
elves.push(currentElf);

const elfFoodTotals : number[] = elves.map(e => e.totalFood());
elfFoodTotals.sort((a,b)=> b-a);

const e1 = elfFoodTotals[0];
const e2 = elfFoodTotals[1];
const e3 = elfFoodTotals[2];
console.log(e1);
console.log(e2);
console.log(e3);
console.log(e1+e2+e3);