// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

let days1To10 () =

    printfn $"\nDay1.1: {Day1.input |> Day1.calculateIncreases}"
    printfn $"Day1.2: {Day1.input |> Day1.calculateGroupIncrease 3}"

    printfn $"\nDay2.1: {Day2.input |> Day2.resultingPosition}"
    printfn $"Day2.2: {Day2.input |> Day2.resultingPositionWithAim}"

    printfn $"\nDay3.1: {Day3.input |> Day3.calculatePowerConsumption}"
    printfn $"Day3.2: {Day3.input |> Day3.calculateLifeSupport}"

    printfn $"\nDay4.1: {Day4.input ||> Day4.playBingo ||> Day4.findScore}"
    printfn $"Day4.2: {Day4.input ||> Day4.looseBingo ||> Day4.findScore}"

    printfn $"\nDay5.1: {Day5.input |> Day5.createPlaneOfHorAndVerVents |> Day5.findOverlapCount 2}"
    printfn $"Day5.2: {Day5.input |> Day5.createPlaneOfVents |> Day5.findOverlapCount 2}"

    printfn $"\nDay6.1: {Day6.input |> Day6.iterDays 80 |> Day6.countFish}"
    printfn $"Day6.1: {Day6.input |> Day6.iterDays 256 |> Day6.countFish}"

    printfn $"\nDay7.1: {Day7.input |> Day7.findAlignment (id)}"
    printfn $"Day7.2: {Day7.input |> Day7.findAlignment (Day7.sum1ToN)}"

    printfn $"\nDay8.1: {Day8.input |> Day8.count1s4s7s8s}"
    printfn $"Day8.2: {Day8.input |> Day8.sumOutputValues}"

    printfn $"\nDay9.1: {Day9.input |> Day9.findLowValues |> Day9.riskLevel}"
    //printfn $"\nDay9.2: {Day9.inputTest |> Day9.findBasins}"

    printfn $"\nDay10.1: {Day10.input |> Day10.sytaxErrorScore}"
    printfn $"Day10.2: {Day10.input |> Day10.autoCompleteScore}"

[<EntryPoint>]
let main argv =
    let message = from "F#" // Call the function
    printfn "Hello world %s" message
    
    //days1To10 ()

    //printfn $"\nDay11.1: {Day11.input |> Day11.stepN 100 |> snd}"
    //printfn $"Day11.2: {Day11.input |> Day11.stepsTillSync 0}"
    
    //printfn $"\nDay15.1: {Day15.input |> Day15.findPath}"
    //printfn $"Day15.2: {Day15.input |> Day15.expandMap |> Day15.findPath}"
        
    0 // return an integer exit code