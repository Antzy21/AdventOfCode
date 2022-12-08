import { dir } from 'console';
import * as fs from 'fs';

const input = fs.readFileSync("input8.txt", "utf-8");
const inputDemo = `30373
25512
65332
33549
35390`

function parseInput(input: string) {
    return input.split('\n').map(row => {
        return row.split('').map(i => {
            return parseInt(i);
        });
    })
}

function isVisibile(visible: boolean, tree: number, otherTree: number) {
    if (!visible || otherTree >= tree) {
        return false;
    }
    else {
        return true;
    }
}

function checkVisibilityFromAllAngles(
    input: number[][],
    row: number[],
    i: number,
    j: number,
    treeHeight: number
) {
    let visibleLeft: boolean =
        [...input[i]]
        .splice(0,j)
        .reduce((isVisible, otherTree) => {
            return isVisibile(isVisible, treeHeight, otherTree)
        }, true);
    let visibleRight: boolean =
        [...input[i]]
        .splice(j+1,row.length)
        .reduce((isVisible, otherTree) => {
            return isVisibile(isVisible, treeHeight, otherTree)
        }, true);
    let visibleDown: boolean =
        [...input].splice(i+1,input.length)
        .map(row => row[j])
        .reduce((isVisible, otherTree) => {
            return isVisibile(isVisible, treeHeight, otherTree)
        }, true);
    let visibleUp: boolean =
        [...input].splice(0,i)
        .map(row => row[j])
        .reduce((isVisible, otherTree) => {
            return isVisibile(isVisible, treeHeight, otherTree)
        }, true);
    return visibleLeft || visibleRight || visibleUp || visibleDown;
}

function getScenicValue(treeheight, list) {
    let v = 0
    let blocked = false;
    list.forEach(tree => {
        if (blocked) {
            
        }
        else if (tree >= treeheight) {
            v ++;
            blocked = true;
        }
        else {
            v ++;
        }
    });
    return v;
}

function scenicScoreFromAllAngles(
    input: number[][],
    row: number[],
    i: number,
    j: number,
    treeHeight: number
) {
    //console.log(`tree is at (${i}, ${j}):`, treeHeight)
    let left = getScenicValue(treeHeight, [...input[i]].splice(0,j).reverse());
    let right = getScenicValue(treeHeight, [...input[i]].splice(j+1,row.length));
    let down = getScenicValue(treeHeight, [...input].splice(i+1,input.length).map(row => row[j]));
    let up = getScenicValue(treeHeight, [...input].splice(0,i).map(row => row[j]).reverse());
    //console.log('u', 'l', 'd', 'r')
    //console.log(up, left, down, right)
    return left * right * up * down;
}

function mapInput(input: number[][], func) {
    let inputCopy = [...input];
    return inputCopy.map((row: number[], i) => {
        let rowCopy = [...row];
        return rowCopy.map((tree: number, j) => {
            return func(input, row, i, j, tree);
        });
    });
}

function countVisible(input: boolean[][]) {
    return input.reduce((acc, row) => {
        let rowV = 
            row.reduce((acc2, i) => {
                if (i) {
                    acc2 ++;
                }
                return acc2;
            }, 0)
        return acc + rowV;
    }, 0)
}

function max(input: number[][]) {
    return input.reduce((acc, row) => {
        let rowMax = 
            row.reduce((acc2, i) => {
                if (i > acc2) {
                    return i;
                }
                return acc2;
            }, 0)
        if (rowMax > acc) {
            return rowMax;
        }
        return acc;
    }, 0)
}

const parsedInput = parseInput(input);
console.log(parsedInput);
const visibleTrees = mapInput(parsedInput, checkVisibilityFromAllAngles);
//console.log(visibleTrees);
const visibleCount = countVisible(visibleTrees);
//console.log(visibleCount);

const treeScenicScores = mapInput(parsedInput, scenicScoreFromAllAngles);
console.log(treeScenicScores)
const treeMaxScenicScore = max(treeScenicScores)
console.log(treeMaxScenicScore)
