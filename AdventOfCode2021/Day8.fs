module Day8

open System.IO
open System

type Entry = (string [] * string [])
type DispPos =
    | Top
    | TopRight
    | TopLeft
    | Middle
    | Bot
    | BotRight
    | BotLeft
type DispMap = Map<DispPos, char>

let private parseInputs (input: string []) : Entry [] =
    input
    |> Array.map (fun s ->
        s.Trim().Split('|')
        |> Array.map (fun s -> s.Trim().Split(' '))
        |> fun ary -> (ary.[0], ary.[1])
    )

let private combineEntry (entry: Entry) = Array.append (fst entry) (snd entry)

let inputTestTest = parseInputs [|"acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf"|]

let inputTest = parseInputs [|
    "be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe"
    "edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc"
    "fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg"
    "fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb"
    "aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea"
    "fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb"
    "dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe"
    "bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef"
    "egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb"
    "gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce"
|]

let input =
    File.ReadLines "inputs/day8.txt"
    |> Seq.toArray 
    |> parseInputs

let count1s4s7s8s (input: Entry []) =
    input
    |> Array.fold (fun count row ->
        snd row
        |> Array.fold (fun count str ->
            match str.Length with
            | 2
            | 3
            | 4
            | 7 -> count + 1
            | _ -> count
        ) 0
        |> (+)count
    ) 0

let private printConfiguration (dispMap: DispMap) =
    printfn $" {dispMap.[Top]}{dispMap.[Top]}{dispMap.[Top]}{dispMap.[Top]} "
    printfn $"{dispMap.[TopLeft]}    {dispMap.[TopRight]}"
    printfn $"{dispMap.[TopLeft]}    {dispMap.[TopRight]}"
    printfn $" {dispMap.[Middle]}{dispMap.[Middle]}{dispMap.[Middle]}{dispMap.[Middle]} "
    printfn $"{dispMap.[BotLeft]}    {dispMap.[BotRight]}"
    printfn $"{dispMap.[BotLeft]}    {dispMap.[BotRight]}"
    printfn $" {dispMap.[Bot]}{dispMap.[Bot]}{dispMap.[Bot]}{dispMap.[Bot]} \n"

let calculateConfiguration (entry: Entry) =
    let dispMap : DispMap = Map.empty<DispPos, char>
    let uniqueDigits = fst entry

    let findOfLength n entries = entries |> Array.filter (fun (s: string) -> s.Length = n)
    let containsNumber (containedNum: string) (num: string) = containedNum |> Seq.forall (num.Contains)
    let containedByNumber (num: string) (containedNum: string) = containedNum |> Seq.forall (num.Contains)
    let findCharDifference (num: string) (largeNum: string) = largeNum |> Seq.find (num.Contains >> not)
    let filterNumber n = Array.filter (fun s -> s = n |> not)

    let one = uniqueDigits |> findOfLength 2 |> Array.head
    let four = uniqueDigits |> findOfLength 4 |> Array.head
    let seven = uniqueDigits |> findOfLength 3 |> Array.head

    let eight = uniqueDigits |> findOfLength 7 |> Array.head
    let nine = uniqueDigits |> findOfLength 6 |> Array.find (containsNumber four)
    let zero = uniqueDigits |> findOfLength 6 |> filterNumber nine |> Array.find (containsNumber one)
    let six = uniqueDigits |> findOfLength 6 |> filterNumber nine |> filterNumber zero |> Array.head

    let dispMap = 
        dispMap
        |> Map.add Top (findCharDifference one seven)
        |> Map.add BotLeft (findCharDifference nine eight)
        |> Map.add Middle (findCharDifference zero eight)
        |> Map.add TopRight (findCharDifference six eight)
    let dispMap = 
        dispMap
        |> Map.add BotRight (findCharDifference $"{dispMap.[TopRight]}" one)
    dispMap
    |> Map.add TopLeft (findCharDifference $"{dispMap.[Middle]}{dispMap.[TopRight]}{dispMap.[BotRight]}" four)
    |> Map.add Bot (findCharDifference $"{four}{dispMap.[Top]}" nine)

let calculateConfigurations (entries: Entry []) = Array.map (calculateConfiguration) entries

let private parseNumWithMap (dispMap: DispMap) (str: string) : int =
    let sortString (str: string) : string = str |> Seq.sort |> String.Concat
    let s = str |> sortString
    match s.Length with
    | 2 -> 1
    | 3 -> 7
    | 4 -> 4
    | 5 ->
        let two = $"{dispMap.[Top]}{dispMap.[TopRight]}{dispMap.[Middle]}{dispMap.[BotLeft]}{dispMap.[Bot]}" |> sortString
        match s = two with
        | true -> 2
        | false ->
            let three = $"{dispMap.[Top]}{dispMap.[TopRight]}{dispMap.[Middle]}{dispMap.[BotRight]}{dispMap.[Bot]}" |> sortString
            match s = three with
            | true -> 3
            | false -> 
                let five = $"{dispMap.[Top]}{dispMap.[TopLeft]}{dispMap.[Middle]}{dispMap.[BotRight]}{dispMap.[Bot]}" |> sortString
                match s = five with
                | true -> 5
                | _ -> failwith "not compatable"
    | 6 ->
        let zero = $"{dispMap.[Top]}{dispMap.[TopRight]}{dispMap.[TopLeft]}{dispMap.[BotLeft]}{dispMap.[BotRight]}{dispMap.[Bot]}" |> sortString
        match s = zero with
        | true -> 0
        | false ->
            let nine = $"{dispMap.[Top]}{dispMap.[TopLeft]}{dispMap.[TopRight]}{dispMap.[Middle]}{dispMap.[BotRight]}{dispMap.[Bot]}" |> sortString
            match s = nine with
            | true -> 9
            | false -> 
                let six = $"{dispMap.[Top]}{dispMap.[TopLeft]}{dispMap.[Middle]}{dispMap.[BotLeft]}{dispMap.[Bot]}{dispMap.[BotRight]}" |> sortString
                match s = six with
                | true -> 6
                | _ -> failwith "not compatable"
    | 7 -> 8
    | _ -> failwith "string too long/short"

let sumOutputValues (entries: Entry []) =
    entries |>
    Array.sumBy (fun entry ->
        let dispMap = calculateConfiguration entry 
        snd entry
        |> Array.map (parseNumWithMap dispMap)
        |> Array.fold (fun n i -> 10*n+i) 0
    )
