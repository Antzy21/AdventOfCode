module Day7

open System.IO

type Crabs = int []

let private parseInputs (input: string []) : Crabs =
    input
    |> Array.head
    |> (fun s -> s.Split(','))
    |> Array.map int

let private printCrabs crabs = printfn "%A" crabs

let inputTest = parseInputs [|
    "16,1,2,0,4,2,7,1,2,14"
|]

let input =
    File.ReadLines "inputs/day7.txt"
    |> Seq.toArray 
    |> parseInputs

let private calculateFuelCost (func: int -> int) crabs horPos : int =
    crabs
    |> Array.fold (fun s crab ->
       (crab - horPos) |> abs |> func |> (+)s
    ) 0

let sum1ToN n = n*(n+1)/2

let findAlignment (fuelCalculator) (crabs: Crabs) =
    let average = 
        crabs
        |> Array.sum
        |> (fun x -> x/(crabs.Length))
    [|(Array.min crabs) .. (Array.max crabs)|]
    |> Array.map (calculateFuelCost (fuelCalculator) crabs)
    //|> Array.map (fun x -> printfn $"{x}"; x)
    |> Array.min

