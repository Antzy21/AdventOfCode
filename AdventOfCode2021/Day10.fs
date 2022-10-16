module Day10

open System
open FSharp.Extensions
open System.IO

let inputTest = [|
    "[({(<(())[]>[[{[]{<()<>>";
    "[(()[<>])]({[<{<<[]>>(";
    "{([(<{}[<>[]}>{[]{[(<()>";
    "(((({<>}<{<{<>}{[]{[]{}";
    "[[<[([]))<([[{}[[()]]]";
    "[{[{({}]{}}([{[{{{}}([]";
    "{<[[]]>}<{[{[{[]{()[[[]";
    "[<(<(<(<{}))><([]([]()";
    "<{([([[(<>()){}]>(<<{{";
    "<{([{{}}[<[[[<>{}]]]>[]]";
|]

let input = 
    File.ReadLines "inputs/day10.txt"
    |> Array.ofSeq

let removeSubStrings (str: string) =
    //printfn $"{str}"
    str 
    |> Seq.map (string)
    |> Seq.reduceBack (fun c s ->
        (c+s)
        |> List.ofSeq
        |> function
        | c1::c2::restOfString when "()" = $"{c1}{c2}" -> 
            //printfn $"removing () from {(c+s)}"
            restOfString
        | c1::c2::restOfString when "[]" = $"{c1}{c2}" ->
            //printfn $"removing [] from {(c+s)}"
            restOfString
        | c1::c2::restOfString when "{}" = $"{c1}{c2}" ->
            //printfn "removing {} from %s" (c+s)
            restOfString
        | c1::c2::restOfString when "<>" = $"{c1}{c2}" ->
            //printfn $"removing <> from {(c+s)}"
            restOfString
        | a -> a
        |> String.ofSeq 
    )

let rec simplify (line: string)=
    let simplifiedLine = 
        line
        |> removeSubStrings

    if simplifiedLine = line then
        simplifiedLine
    else
        simplify simplifiedLine

let private firstIllegalChar (str: string) : char option=
    str |> Seq.tryFind (function
        | ')' | '>' | ']' | '}' -> true
        | _ -> false
    )

let sytaxErrorScore (lines : string []) =
    lines
    |> Array.map (simplify >> firstIllegalChar)
    |> Array.fold (fun i c ->
        match c with
        | Some ')' -> i+3
        | Some ']' -> i+57
        | Some '}' -> i+1197
        | Some '>' -> i+25137
        | None -> i
        | Some _ -> failwith "Not correct bracket"
    ) 0

let autoCompleteScore (lines : string []) =
    lines
    |> Array.map (simplify)
    |> Array.filter (firstIllegalChar >> Option.isNone)
    |> Array.map (fun line ->
        let v = 
            line
            |> Seq.rev
            |> Seq.fold (fun i c ->
                match c with
                | '(' -> (i*5.)+1.
                | '[' -> (i*5.)+2.
                | '{' -> (i*5.)+3.
                | '<' -> (i*5.)+4.
                | _ -> failwith "Not correct bracket"
            ) ((double)0)
        printfn "%s %A" line v
        v
    )
    |> Array.sortDescending
    |> (fun ary -> ary.[(ary.Length/2)])