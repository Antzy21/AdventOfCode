import * as fs from 'fs';

let input = fs.readFileSync("input1.txt", "utf-8");
let inputDemo = `1000
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
        let total : number = 0;
        this.foods.forEach(food => {
            total += food
        });
        return total;
    }
}

let lines = input.split('\n')

let currentElf = new elf();
let elves : elf[] = [];

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

let elfFoodTotals : number[] = elves.map(e => e.totalFood());
elfFoodTotals.sort((a,b)=> b-a);

let e1 = elfFoodTotals[0];
let e2 = elfFoodTotals[1];
let e3 = elfFoodTotals[2];
console.log(e1);
console.log(e2);
console.log(e3);
console.log(e1+e2+e3);