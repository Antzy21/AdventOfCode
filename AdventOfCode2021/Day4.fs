module Day4

open System
open System.IO

type private BingoBoard = (bool * int) [] []

let private boardMap func (board: BingoBoard) =
    board
    |> Array.map (fun row ->
        row
        |> Array.map func
    )

let private printBoard board = 
    board |> Array.iter (fun row -> printfn "%A" row)
    printfn ""

let private stringToIntList (charToSplit: char) (s: string) =
    s.Split(charToSplit)
    |> Array.filter (fun s -> s <> "")
    |> Array.map (Int32.Parse)

let private parseInputs (input: string []) : (int [] * BingoBoard []) =
    let nums = Array.head input |> stringToIntList ','
    let boards : BingoBoard [] =
        Array.tail input
        |> Array.splitInto (input.Length / 6)
        |> Array.map (fun chunk ->
            Array.tail chunk //ignore blank line at the start
            |> Array.map (fun s -> 
                stringToIntList ' ' s
                |> Array.map (fun n -> (false, n))
            )
        )
    (nums, boards)

let input = 
    File.ReadLines "inputs/day4.txt"
    |> Seq.toArray 
    |> parseInputs

let inputTest = parseInputs [|
    "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1";
    "";
    "22 13 17 11  0";
    " 8  2 23  4 24";
    "21  9 14 16  7";
    " 6 10  3 18  5";
    " 1 12 20 15 19";
    "";
    "3 15  0  2 22";
    "9 18 13 17  5";
    "19  8  7 25 23";
    "20 11 10 24  4";
    "14 21 16 12  6";
    "";
    "14 21 17 24  4";
    "10 16 15  9 19";
    "18  8 23 26 20";
    "22 11 13  6  5";
    "2  0 12  3  7";
|]

let rec private transpose = function
    | (_::_)::_ as matrix -> 
        List.map List.head matrix :: transpose (List.map List.tail matrix)
    | _ -> []

let private boardAsList board = List.ofArray (board |> Array.map List.ofArray)
let private boardAsArray board = Array.ofList (board |> List.map Array.ofList)

let private transposeBoard = boardAsList >> transpose >> boardAsArray


let private checkIfBoardHasWon (board: BingoBoard) : bool =
    // Check rows:
    let rowChecks board = 
        board
        |> Array.fold (fun s row ->
            let rowHasWon =
                row
                |> Array.fold (fun s cell->
                    fst cell && s
                ) true
            rowHasWon || s
        ) false
    
    let transposedBoard = transposeBoard board

    let hasWon = rowChecks board || rowChecks transposedBoard
    match hasWon with
    | true ->
        //printfn "Bingo!"
        true
    | fail -> false

let private checkBoardForNum (n: int) (board: BingoBoard) =
    board
    |> boardMap (fun cell ->
        match (snd cell) = n with
        | true -> (true, snd cell)
        | false -> cell
    )

let findScore (board: BingoBoard) (n: int) : int =
    //printBoard board
    board 
    |> Array.collect (fun row ->
        row
        |> Array.filter (fst >> not)
        |> Array.map snd
    )
    |> Array.sum
    |> (fun sum -> sum * n)

type State = {Boards: BingoBoard []; Winner: BingoBoard option; CurrentN: int}

let private callNthenCheckForWinner (state: State) (n: int) =
    let boards = state.Boards
    let winningBoard = state.Winner
    match winningBoard with
    | Some _ -> state
    | None -> 
        //printfn $"{n}"
        let boards = 
            boards
            |> Array.map (fun board -> checkBoardForNum n board)
        boards
        |> Array.fold (fun winner board ->
            match winner with
            | Some _ -> winner
            | None -> 
                match checkIfBoardHasWon board with
                | true -> Some board
                | false -> None
        ) None
        |> (fun winner -> {Boards = boards; Winner = winner; CurrentN = n})

let playBingo (nums: int []) (boards: BingoBoard []) =
    nums
    |> Array.fold callNthenCheckForWinner {Boards = boards; Winner = None; CurrentN = 0}
    |> (fun (s: State) -> 
        (
            s.Winner |> Option.defaultWith (fun () -> failwith "NoWinner"),
            s.CurrentN)
        )

let private plainBoard b = boardMap snd b

let rec looseBingo (nums: int []) (boards: BingoBoard []) =
    match boards.Length with
    | 1 -> 
        boards 
        |> playBingo nums
    | _ -> 
        playBingo nums boards
        |> fst
        |> (fun winner ->
            let boards = 
                Array.filter (fun b ->
                    plainBoard b <> plainBoard winner
                ) boards
            looseBingo nums boards
        )