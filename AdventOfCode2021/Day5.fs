module Day5

open System.IO
open FSharp.Extensions

type Line = ((int * int) * (int * int))

type Plane = int [,]

let private parseInputs (input: string []) : Line [] =
    input
    |> Array.map (fun s ->
        s.Split(' ')
        |> Array.filter (fun s -> s <> "->")
        |> (fun x1y1x2y2 ->
            let x1, y1 = 
                x1y1x2y2
                |> Array.head
                |> (fun s -> s.Split(','))
                |> Array.map int
                |> (fun ary -> (Array.head ary, Array.tail ary |> Array.head ))
            let x2, y2 = 
                x1y1x2y2
                |> Array.tail |> Array.head 
                |> (fun s -> s.Split(','))
                |> Array.map int
                |> (fun ary -> (Array.head ary, Array.tail ary |> Array.head ))
            ((x1, y1), (x2, y2))
        )
    )

let private x1 = fst >> fst
let private y1 = fst >> snd
let private x2 = snd >> fst
let private y2 = snd >> snd

let inputTest = parseInputs [|
    "0,9 -> 5,9";
    "8,0 -> 0,8";
    "9,4 -> 3,4";
    "2,2 -> 2,1";
    "7,0 -> 7,4";
    "6,4 -> 2,0";
    "0,9 -> 2,9";
    "3,4 -> 1,4";
    "0,0 -> 8,8";
    "5,5 -> 8,2";
|]

let input =
    File.ReadLines "inputs/day5.txt"
    |> Seq.toArray 
    |> parseInputs

type private LineType =
    | Hor
    | Ver
    | DiagPos
    | DiagNeg

let private print cell =
    match cell with
    | 0 -> "."
    | x -> $"{x}"

let private fold2D (folder: 'S -> 'T -> 'S) (state: 'S) (array: 'T[,]) =
    let mutable state = state
    for x in 0 .. Array2D.length1 array - 1 do
        for y in 0 .. Array2D.length2 array - 1 do
            state <- folder state (array.[x, y])
    state

let private printLine (line: Line) = printfn $"{x1 line}, {y1 line}, {x2 line}, {y2 line}"
let private printPlane (plane: Plane) =
    plane
    |> Array2D.iteri (fun _ j cell ->
        match j = (Array2D.base2 plane) with
        | true -> printfn ""; printf $"{print cell}"
        | false -> printf $"{print cell}"
    )
    printfn "\n"

let private lineType line =
    if (line |> y1) = (line |> y2) then
        Ver
    elif (line |> x1) = (line |> x2) then
        Hor
    elif ((x1 line) < (x2 line) && (y1 line) < (y2 line)) || (x2 line) < (x1 line) && (y2 line) < (y1 line) then
        DiagPos
    else
        DiagNeg

let private horAndVerLines (lines: Line []) : Line [] =
    lines
    |> Array.filter (fun line ->
        lineType line = Hor || lineType line = Ver
    )

let private addDotToPlain (plane: Plane) coordinates =
    plane.[fst coordinates, snd coordinates] <- plane.[fst coordinates, snd coordinates]+1
    plane

let private addLineToPlane (plain: Plane) (line: Line) = 
    match lineType line with
    | Hor ->
        let ys = min (y1 line) (y2 line)
        let ye = max (y1 line) (y2 line)
        [|ys .. ye|]
        |> Array.map (fun y -> (x1 line, y))
        |> Array.fold addDotToPlain plain
        //|> (fun p -> printLine line; printPlane p; p)
    | Ver ->
        let xs = min (x1 line) (x2 line)
        let xe = max (x1 line) (x2 line)
        [|xs .. xe|]
        |> Array.map (fun x -> (x, y1 line))
        |> Array.fold addDotToPlain plain
        //|> (fun p -> printLine line; printPlane p; p)
    | DiagPos -> 
        let xs = min (x1 line) (x2 line)
        let xe = max (x1 line) (x2 line)
        let ys = min (y1 line) (y2 line)
        let ye = max (y1 line) (y2 line)
        let xList = [|xs .. xe|]
        let yList = [|ys .. ye|]
        Seq.zip xList yList
        |> Array.ofSeq
        |> Array.fold addDotToPlain plain
    | DiagNeg ->
        let xs = min (x1 line) (x2 line)
        let xe = max (x1 line) (x2 line)
        let ys = min (y1 line) (y2 line)
        let ye = max (y1 line) (y2 line)
        let xList = [|xs .. xe|] |> Array.rev
        let yList = [|ys .. ye|]
        Seq.zip xList yList
        |> Array.ofSeq
        |> Array.fold addDotToPlain plain

let private getPlane lines : Plane =
    lines
    |> Array.fold (fun p line ->
        let maxX = 1 + max (x1 line) (x2 line)
        let maxY = 1 + max (y1 line) (y2 line)
        (max (fst p) maxX, max (snd p) maxY)
    ) (0,0)
    ||> Array2D.zeroCreate

let findOverlapCount (overLapOf: int) (plane: Plane) : int =
    plane
    |> Array2D.fold (fun s cell ->
        match cell >= overLapOf with
        | true -> s+1 
        | false -> s
    ) 0

let createPlaneOfVents (lines: Line []) =
    let plane = getPlane lines
    lines
    |> Array.fold addLineToPlane plane

let createPlaneOfHorAndVerVents (lines : Line []) =
    horAndVerLines lines
    |> createPlaneOfVents
    