module Day11

open System
open FSharp.Extensions
open System.IO

let private parseInput = Array.map (Seq.map ((string) >> Int32.Parse) >> Array.ofSeq) >> array2D

let inputTest = parseInput [|
    "5483143223"
    "2745854711"
    "5264556173"
    "6141336146"
    "6357385478"
    "4167524645"
    "2176841721"
    "6882881134"
    "4846848554"
    "5283751526"
|]

let input = 
    File.ReadLines "inputs/day11.txt"
    |> Array.ofSeq
    |> parseInput

let private adjacentFlashEffect (energyLevels: int [,]) : int [,] =
    let copy = Array2D.copy energyLevels
    energyLevels
    |> Array2D.foldij (fun i j (levels: int [,]) level ->
        if level > 9 then
            levels.[i,j] <- -100 // Has flashed, don't flash again
            levels
            |> Array2D.modifyAdjacent true ((+)1) i j
        else
            levels
    ) copy

let private step (energyLevels: int [,]) =
    let energyLevels = energyLevels |> Array2D.map ((+)1)
    let rec handleFlashes energyLevels =
        if energyLevels |> Array2D.exists (fun x -> x>9) then
            //printfn "Still contains 10s"
            energyLevels
            |> adjacentFlashEffect
            |> handleFlashes
        else
            energyLevels
    handleFlashes energyLevels
    |> Array2D.map (fun level ->
        match level > 0 with
        | true -> level
        | false -> 0
    )
    //|> Array2D.printPass

let stepN n energyLevels =
    [|1..n|]
    |> Array.fold (fun (levels, flashCount) _ ->
        let levels = step levels
        let flashCount = flashCount + Array2D.count ((=)0) levels
        //printfn $"{flashCount}"
        (levels, flashCount)
    ) (energyLevels, 0)

let rec stepsTillSync n energyLevels =
    if Array2D.count ((=)0) energyLevels = 100 then
        n
    else
        step energyLevels
        |> stepsTillSync (n+1)