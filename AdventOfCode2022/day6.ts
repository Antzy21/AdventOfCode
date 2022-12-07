import * as fs from 'fs';

const input = fs.readFileSync("input6.txt", "utf-8");
const inputDemo = `mjqjpqmgbljsphdztnvjfqwrcgsmlb`

function isUnique(input: string) {
    const inputArray = input.split('');
    const l = input.length;
    for (let i = 0; i < l; i++) {
        let v = inputArray.pop()
        if (v != undefined && inputArray.includes(v)) {
            return false
        }
    }
    return true
}

function isUniqueAt(input: string, length: number, pos: number) {
    const charArray = input.slice(pos, pos+length);
    return isUnique(charArray);
}

function solve(input: string, length: number) {
    let difCharCount = 0
    let pos = 0
    while (!isUniqueAt(input, length, pos)) {
        pos ++;
    }
    return length+pos;
}

const res1 = solve(input, 4);
console.log(res1);

const res2 = solve(input, 14);
console.log(res2);