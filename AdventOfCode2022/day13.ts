import * as fs from 'fs';

const input = fs.readFileSync("input13.txt", "utf-8").trim();
const inputDemo = 
`[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]`

function parseInput(input: string) {
    return input.split('\n\n').map(l => l.split('\n'));
}

function parseList(listString: string) : any[] {
    let listDepth = 0;
    let lists = listString.split('').splice(1,listString.length-2)
    let res = lists
        .reduce((lst: any[], char: string) => {
            if (char == "[") {
                if (listDepth == 0) {
                    lst.push("")
                }
                listDepth ++
            }
            if (listDepth == 0) {
                if (char == ",") {
                }
                else if (!Number.isNaN(parseInt(char))) {
                    lst.push(parseInt(char))
                }
            }
            else {
                lst[lst.length-1] += char
            }
            if (char == "]") {
                listDepth --
                if (listDepth != 0) {
                    //lst[lst.length - 1] += char;
                }
            }
            return lst;
        }, [])
    return res.map(lst => {
        if (typeof lst == "string" && lst != "") {
            return parseList(lst);
        }
        else {
            return lst;
        }
    })
}

function listsAreInCorrectOrder(x: any, y: any, log = false, indent = 0) : number {
    if (x === y) {
        if (log) {console.log("  ".repeat(indent), "Compare", x, "vs", y)}
        return 0;
    }
    else if (x == undefined) {
        if (log) {console.log("  ".repeat(indent), "Left side ran out of items - in the right order")}
        return 1
    }
    else if (y == undefined) {
        if (log) {console.log("  ".repeat(indent), "Right side ran out of items - not")}
        return -1
    }
    else if (log) {console.log("  ".repeat(indent), "Compare", x, "vs", y)}
    if (typeof x == "number") {
        if (typeof y == "number") {
            if (x < y) {
                if (log) {console.log("  ".repeat(indent+1), "Left side is smaller - in the right order")}
                return 1;
            }
            else {
                if (log) {console.log("  ".repeat(indent+1), "Right side is smaller - not")}
                return -1;
            }
        }
        else {
            if (log) {console.log("  ".repeat(indent+1), "Mixed type; convert left to", [x])}
            return listsAreInCorrectOrder([x], y, log, indent+1)
        }
    } else {
        if (typeof y == "number") {
            if (log) {console.log("  ".repeat(indent+1), "Mixed type; convert right to", [y])}
            return listsAreInCorrectOrder(x, [y], log, indent+1)
        }
        else {
            let i = 0
            let v = 0
            while (v == 0 && i < Math.max(x.length, y.length)) {
                v = listsAreInCorrectOrder(x[i],y[i], log, indent+1)
                i++
            }
            return v;
        }
    }
}

function solve(input: string, log = false) {
    const lists = parseInput(input);
    //console.log(lists)
    return lists.reduce((acc, list, i) => {
        let x = parseList(list[0]);
        let y = parseList(list[1]);
        if (log) {console.log("== Pair",i+1,"==")}
        if (listsAreInCorrectOrder(x,y, log) > 0) {
            acc += i+1
            console.log(i+1)
        }
        return acc;
    }, 0);
}

//console.log(solve(inputDemo, true))
console.log(solve(input, true))