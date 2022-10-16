module Day1

open System.IO

let calculateIncreases (depths: int seq) : int =
    let initState = (0, Seq.head depths)
    let folder state (newDepth: int) =
        let count = fst state
        match newDepth > (snd state) with
        | true -> (count+1, newDepth)
        | false -> (count, newDepth)
    depths
    |> Seq.fold folder initState
    |> fst

let calculateGroupIncrease (increaseCount : int) (depths: int array) : int =
    let initState = (0, depths.[0..increaseCount-1])
    let folder state (newDepth: int) =
        let count = fst state
        let previousSet : int array = snd state
        let previousSetSize = Seq.sum previousSet
        let newSet = Array.append previousSet.[1..] [|newDepth|]
        let newSetSize = Seq.sum newSet
        match newSetSize > previousSetSize with
        | true -> (count+1, newSet)
        | false -> (count, newSet)
    depths
    |> Seq.fold folder initState
    |> fst

let input =
    File.ReadLines "inputs/day1.txt"
    |> Seq.map int
    |> Seq.toArray

let inputTest = [|
    199
    200
    208
    210
    200
    207
    240
    269
    260
    263
|]