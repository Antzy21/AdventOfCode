module Day3

open System
open System.IO

let input =
    File.ReadLines "inputs/day3.txt"
    |> Seq.toArray    

let inputTest = [|
    "00100";
    "11110";
    "10110";
    "10111";
    "10101";
    "01111";
    "00111";
    "11100";
    "10000";
    "11001";
    "00010";
    "01010";
|]

let private processRow (state: int []) (row: string) =
    row
    |> Array.ofSeq
    |> Array.map (fun b -> 
        match b with
        | '0' -> -1
        | '1' -> 1
        | _ -> failwith "invalid"
    )
    |> Array.zip state
    |> Array.map (fun tup -> fst tup + snd tup)

let private calculateSumOfBits diagnosticReport =
    let initState = diagnosticReport |> Array.head |> String.length |> Array.zeroCreate<int> 
    diagnosticReport
    |> Array.fold processRow initState

let private calculateGammaRate sumOfBits =
    sumOfBits
    |> Array.map (fun b -> 
        match b >= 0 with
        | true -> 1
        | false -> 0
    )

let private intArrayToString ary =
    ary
    |> Array.map (fun x -> $"{x}")
    |> String.Concat

let private binaryToInt bin = 
    try 
        Convert.ToInt32(bin, 2)
    with 
    | _ ->  failwith $"{bin}"

let private calculateEpsilonRate sumOfBits =
    sumOfBits
    |> Array.map (fun b -> 
        match b >= 0 with
        | true -> 0
        | false -> 1
    )
    
let private removeFirstChars (ary: string []) : string [] =
    ary
    |> Array.map (Seq.tail >> String.Concat)

let calculatePowerConsumption diagnosticReport : int =
    let sumOfBits = calculateSumOfBits diagnosticReport
    let gamma = calculateGammaRate sumOfBits |> intArrayToString
    let epsilon = calculateEpsilonRate sumOfBits |> intArrayToString
    (binaryToInt gamma) * (binaryToInt epsilon)

let private folder (func: int [] -> int []) (diagnosticReport: string []) n =
    match diagnosticReport.Length with
    | 1 -> diagnosticReport
    | _ ->
        let bitCount = diagnosticReport |> calculateSumOfBits |> func |> (fun bc -> bc.[n])
        diagnosticReport
        |> Array.filter (fun d -> Int32.Parse $"{d.[n]}" = bitCount)

let calculateOxygen diagnosticReport : int =
    [0..(diagnosticReport |> Array.head |> String.length)]
    |> Seq.fold (folder calculateGammaRate) diagnosticReport
    |> Array.head
    |> binaryToInt

let calculateC02 diagnosticReport : int =
    [0..(diagnosticReport |> Array.head |> String.length)]
    |> Seq.fold (folder calculateEpsilonRate) diagnosticReport
    |> Array.head
    |> binaryToInt

let calculateLifeSupport diagnosticsReport : int =
    (calculateC02 diagnosticsReport) * (calculateOxygen diagnosticsReport)