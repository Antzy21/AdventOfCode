import * as fs from 'fs';

let input = fs.readFileSync("input2.txt", "utf-8");
let inputDemo = `A Y
B X
C Z`;

type OppPlay = "A" | "B" | "C"

type Response = "Y" | "X" | "Z"

type PlayChoice = "Rock" | "Paper" | "Scissors"

type Result = "Win" | "Draw" | "Loss"

function scoreFromPlayChoice(playChoice: PlayChoice) : number {
    switch (playChoice) {
        case "Rock":
            return 1;
        case "Paper":
            return 2;
        case "Scissors":
            return 3;
    }
}

function parseOppPlay(oppPlay: OppPlay) : PlayChoice {
    switch (oppPlay) {
        case "A":
            return "Rock";
        case "B":
            return "Paper";
        case "C":
            return "Scissors";
    }
}

function scoreFromResult(result: Result) : number {
    switch (result) {
        case "Win":
            return 6;
        case "Draw":
            return 3;
        case "Loss":
            return 0;
    }
}

function parseResponse(response: Response) : PlayChoice {
    switch (response) {
        case "Y":
            return "Paper";
        case "X":
            return "Rock";
        case "Z":
            return "Scissors";
    }
}

function parseResult(result: Response) : Result {
    switch (result) {
        case "Y":
            return "Draw";
        case "X":
            return "Loss";
        case "Z":
            return "Win";
    }
}

function reverseEngineerPlayChoice(result: Result, opponentChoice: PlayChoice) : PlayChoice {
    switch (result) {
        case "Win":
            switch (opponentChoice) {
                case "Rock":
                    return "Paper";
                case "Paper":
                    return "Scissors";
                case "Scissors":
                    return "Rock";
            }
        case "Draw":
            switch (opponentChoice) {
                case "Rock":
                    return "Rock";
                case "Paper":
                    return "Paper";
                case "Scissors":
                    return "Scissors";
            }
        case "Loss":
            switch (opponentChoice) {
                case "Rock":
                    return "Scissors";
                case "Paper":
                    return "Rock";
                case "Scissors":
                    return "Paper";
            }
    }
}


function playGame(playChoice: PlayChoice, opponentChoice: PlayChoice) : Result {
    switch (playChoice) {
        case "Rock":
            switch (opponentChoice) {
                case "Rock":
                    return "Draw";
                case "Paper":
                    return "Loss";
                case "Scissors":
                    return "Win";
            }
        case "Paper":
            switch (opponentChoice) {
                case "Rock":
                    return "Win";
                case "Paper":
                    return "Draw";
                case "Scissors":
                    return "Loss";
            }
        case "Scissors":
            switch (opponentChoice) {
                case "Rock":
                    return "Loss";
                case "Paper":
                    return "Win";
                case "Scissors":
                    return "Draw";
            }
    }
}

function parseLine(line: string) : [PlayChoice, PlayChoice] {
    const oppPlay = line.charAt(0) as OppPlay
    const response = line.charAt(2) as Response
    return [parseResponse(response), parseOppPlay(oppPlay)]
}

function parseLine2(line: string) : [PlayChoice, PlayChoice] {
    const oppPlayChar = line.charAt(0) as OppPlay
    const result = line.charAt(2) as Response
    const oppPlay = parseOppPlay(oppPlayChar);
    const response = reverseEngineerPlayChoice(parseResult(result), oppPlay)
    return [response, oppPlay]
}

const lines = input.trim().split("\n");

function calcPart1(lines) {
    const result = lines.reduce((acc, line) => {
        const [response, oppPlay] = parseLine(line);
        const result = playGame(response, oppPlay);
        const resultPoints = scoreFromResult(result);
        const playPoints = scoreFromPlayChoice(response);
        //console.log(`${line} => oppPlay: ${oppPlay} vs response: ${response} => ${resultPoints} + ${playPoints} = ${score+playPoints}`);
        return acc+resultPoints+playPoints;
    }, 0);
    return result;
}
function calcPart2(lines) {
    const result = lines.reduce((acc, line) => {
        const [response, oppPlay] = parseLine2(line);
        const result = playGame(response, oppPlay);
        const resultPoints = scoreFromResult(result);
        const playPoints = scoreFromPlayChoice(response);
        //console.log(`${line} => oppPlay: ${oppPlay} vs response: ${response} => ${resultPoints} + ${playPoints} = ${score+playPoints}`);
        return acc+resultPoints+playPoints;
    }, 0);
    return result;
}

const part1 = calcPart1(lines);
const part2 = calcPart2(lines);
console.log(part1);
console.log(part2);