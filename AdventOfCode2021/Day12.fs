module Day12

open System
open FSharp.Extensions
open System.IO

let private parseInput = Array.map id

let inputTest = parseInput [|
    """start-A
    start-b
    A-c
    A-b
    b-d
    A-end
    b-end"""
|]

let input = 
    File.ReadLines "inputs/day12.txt"
    |> Array.ofSeq
    |> parseInput
