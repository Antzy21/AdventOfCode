module Day6

open System.IO

type Fish = (int * double)[]

let private parseInputs (input: string []) =
    let blanks =
        [|0..8|]
        |> Array.map (fun i -> (i, (double)0))
    input
    |> Array.head
    |> (fun s -> s.Split(','))
    |> Array.map (double)
    |> Array.fold (fun (s: Fish) (i: double) ->
        let i = (int) i
        s.[i] <- (fst s.[i], snd s.[i]+(double)1)
        s
    ) blanks

let private printFish fish = printfn "%A" fish

let inputTest = parseInputs [|
    "3,4,3,1,2"
|]

let input =
    File.ReadLines "inputs/day6.txt"
    |> Seq.toArray 
    |> parseInputs

let private timerIsN n timerAndCount = fst timerAndCount = n

let private getFishWithTimerN n fish =
    fish
    |> Array.filter (timerIsN n)
    |> Array.head

let iterDay (fish: Fish) =
    let pregoFishCount =
        fish
        |> getFishWithTimerN 0
        |> snd

    let i = fish |> Array.findIndex (timerIsN 7)
    fish.[i] <- (fst fish.[i], snd fish.[i] + pregoFishCount) 

    let fish =
        fish
        |> Array.map (fun (timer, fishCount) ->
            match timer with
            | 0 -> (8, fishCount)
            | _ -> (timer-1, fishCount)
        )
    //printFish fish
    fish

let iterDays n fish = [|1..n|] |> Array.fold (fun fish n -> iterDay fish) fish

let countFish (fish: Fish) =
    Array.sumBy snd fish

