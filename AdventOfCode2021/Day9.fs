module Day9

open System
open FSharp.Extensions
open System.IO

type position = int * int
let private parseInput = Array.map (Seq.map ((string) >> Int32.Parse) >> Array.ofSeq) >> array2D

let inputTest = parseInput [|
    "2199943210";
    "3987894921";
    "9856789892";
    "8767896789";
    "9899965678";
|]

let input = 
    File.ReadLines "inputs/day9.txt"
    |> Array.ofSeq
    |> parseInput

let findLowValues (plane: int [,]) : int list =
    plane
    |> Array2D.foldi (fun i j (s: int list) cell ->
        match Array2D.adjacentComparisonCheck plane ( > ) i j with
        | true -> cell :: s
        | false -> s
    ) (List.Empty)

let riskLevel (lowPoints: int list) =
    lowPoints
    |> List.sumBy ((+)1)

let findLowPoints (plane: int [,]) : position list =
    plane
    |> Array2D.foldi (fun i j (s: position list) cell ->
        match Array2D.adjacentComparisonCheck plane ( > ) i j with
        | true -> (i, j) :: s
        | false -> s
    ) (List.Empty)

let private expandBasin (plane: int [,]) (basin: position []) =
    basin
    |> Array.fold (fun (s: position []) (i, j) ->
        let testNewPos state (i, j) =
            match Array2D.tryItem i j plane with
            | Some num -> 
                if Array.contains (i, j) state then
                    state
                else if num = 9 then
                    state
                else
                    Array.append state [|(i, j);|]
            | None -> state
        [|
            ((i+1), j);
            ((i-1), j);
            (i, (j+1));
            (i, (j-1));
        |]
        |> Array.fold testNewPos s
    ) basin

let findBasin plane (lowPoint: position) =
    let expandBasin = expandBasin plane
    let basin = [|lowPoint;|]
    let rec expand basin =
        let expandedBasin = expandBasin basin
        if expandedBasin.Length = basin.Length then
            basin
        else
            expand basin

    expand basin

let findBasins (plane: int [,]) =
    plane
    |> findLowPoints
    |> List.map (findBasin plane)
    |> List.iter (fun a -> printfn $"{a}")
    