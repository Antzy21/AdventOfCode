module Day2

open System.IO

let input =
    File.ReadLines "inputs/day2.txt"
    |> Seq.toArray

let inputTest = [|
    "forward 5"
    "down 5"
    "forward 8"
    "up 3"
    "down 8"
    "forward 2"
|]

let private getValueFromStringLine (typeName: string) (i: string) = i.Remove(0, typeName.Length+1) |> int

let private sumInstructionsOfType (instructions: string array) (typeName: string) =
    instructions
    |> Seq.filter (fun i -> i.Contains typeName)
    |> Seq.sumBy (getValueFromStringLine typeName)

type State = {Aim: int; Depth: int; Hor: int}

let resultingPositionWithAim (instructions: string array) =
    let processInstruction (currentState: State) (instruction: string) : State =
        match (instruction.[0..1]) with
        | "up" -> {
                Aim = currentState.Aim - getValueFromStringLine "up" instruction;
                Depth = currentState.Depth;
                Hor = currentState.Hor;
            }
        | "do" -> {
                Aim = currentState.Aim + getValueFromStringLine "down" instruction;
                Depth = currentState.Depth;
                Hor = currentState.Hor;
            }
        | "fo" -> {
                Aim = currentState.Aim;
                Depth = currentState.Depth + currentState.Aim * getValueFromStringLine "forward" instruction;
                Hor = currentState.Hor + getValueFromStringLine "forward" instruction
            }
        | _ -> failwith "Invalid instruction"
    let initState: State = {Aim = 0; Depth = 0; Hor = 0}
    instructions
    |> Seq.fold processInstruction initState
    |> (fun state -> (state.Depth) * state.Hor)

let resultingPosition instructions =
    let downCount = sumInstructionsOfType instructions "down"
    let upCount = sumInstructionsOfType instructions "up"
    let forwardCount = sumInstructionsOfType instructions "forward"
    (downCount - upCount) * forwardCount