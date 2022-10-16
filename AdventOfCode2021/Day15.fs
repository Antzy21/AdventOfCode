module Day15

open System
open FSharp.Extensions
open System.IO

let private parseInput = Array.map (Seq.map ((string) >> Int32.Parse) >> Array.ofSeq) >> array2D

let inputTest = parseInput [|
    "1163751742"
    "1381373672"
    "2136511328"
    "3694931569"
    "7463417111"
    "1319128137"
    "1359912421"
    "3125421639"
    "1293138521"
    "2311944581"
|]

let inputTest2 = parseInput [|
    "11637517422274862853338597396444961841755517295286"
    "13813736722492484783351359589446246169155735727126"
    "21365113283247622439435873354154698446526571955763"
    "36949315694715142671582625378269373648937148475914"
    "74634171118574528222968563933317967414442817852555"
    "13191281372421239248353234135946434524615754563572"
    "13599124212461123532357223464346833457545794456865"
    "31254216394236532741534764385264587549637569865174"
    "12931385212314249632342535174345364628545647573965"
    "23119445813422155692453326671356443778246755488935"
    "22748628533385973964449618417555172952866628316397"
    "24924847833513595894462461691557357271266846838237"
    "32476224394358733541546984465265719557637682166874"
    "47151426715826253782693736489371484759148259586125"
    "85745282229685639333179674144428178525553928963666"
    "24212392483532341359464345246157545635726865674683"
    "24611235323572234643468334575457944568656815567976"
    "42365327415347643852645875496375698651748671976285"
    "23142496323425351743453646285456475739656758684176"
    "34221556924533266713564437782467554889357866599146"
    "33859739644496184175551729528666283163977739427418"
    "35135958944624616915573572712668468382377957949348"
    "43587335415469844652657195576376821668748793277985"
    "58262537826937364893714847591482595861259361697236"
    "96856393331796741444281785255539289636664139174777"
    "35323413594643452461575456357268656746837976785794"
    "35722346434683345754579445686568155679767926678187"
    "53476438526458754963756986517486719762859782187396"
    "34253517434536462854564757396567586841767869795287"
    "45332667135644377824675548893578665991468977611257"
    "44961841755517295286662831639777394274188841538529"
    "46246169155735727126684683823779579493488168151459"
    "54698446526571955763768216687487932779859814388196"
    "69373648937148475914825958612593616972361472718347"
    "17967414442817852555392896366641391747775241285888"
    "46434524615754563572686567468379767857948187896815"
    "46833457545794456865681556797679266781878137789298"
    "64587549637569865174867197628597821873961893298417"
    "45364628545647573965675868417678697952878971816398"
    "56443778246755488935786659914689776112579188722368"
    "55172952866628316397773942741888415385299952649631"
    "57357271266846838237795794934881681514599279262561"
    "65719557637682166874879327798598143881961925499217"
    "71484759148259586125936169723614727183472583829458"
    "28178525553928963666413917477752412858886352396999"
    "57545635726865674683797678579481878968159298917926"
    "57944568656815567976792667818781377892989248891319"
    "75698651748671976285978218739618932984172914319528"
    "56475739656758684176786979528789718163989182927419"
    "67554889357866599146897761125791887223681299833479"
|]

let input = 
    File.ReadLines "inputs/day15.txt"
    |> Array.ofSeq
    |> parseInput

let rec private pathList (dijkstraMap: int [,]) (costMap: int[,]) (i: int) (j: int) : int [] =
    Array2D.adjacentFindi dijkstraMap (fun cost -> cost = dijkstraMap.[i,j] - costMap.[i,j]) i j
    |> function
    | (0,_) -> [|0|]
    | (_, (i2,j2)) -> Array.append [|costMap.[i2,j2]|] (pathList dijkstraMap costMap i2 j2)

let rec private dijkstraAlgorithm (costMap: int[,]) (dijkstraMap: int[,]) (visited: bool[,]): int =
    // find unvisited with lowest distance
    Array2D.flateni dijkstraMap
    |> Array.ofSeq
    |> Array.filter (fun (_, (i, j)) -> not visited.[i,j])
    |> Array.reduce (fun (dist1, (i1, j1)) (dist2, (i2, j2)) -> 
        let dist1AndPositionAlteration = dist1 - i1 - j1
        let dist2AndPositionAlteration = dist2 - i2 - j2
        if dist1AndPositionAlteration < dist2AndPositionAlteration then
            (dist1, (i1, j1))
        else
            (dist2, (i2, j2))
    )
    ||> (fun dist (i,j) -> 
        let X = Array2D.length1 dijkstraMap - 1
        let Y = Array2D.length2 dijkstraMap - 1
        if i <> 0 then
            dijkstraMap.[i-1,j] <- (min (costMap.[i-1,j] + dist) dijkstraMap.[i-1,j])
        if i <> X then
            dijkstraMap.[i+1,j] <- (min (costMap.[i+1,j] + dist) dijkstraMap.[i+1,j])
        if j <> 0 then
            dijkstraMap.[i,j-1] <- (min (costMap.[i,j-1] + dist) dijkstraMap.[i,j-1])
        if j <> Y then
            dijkstraMap.[i,j+1] <- (min (costMap.[i,j+1] + dist) dijkstraMap.[i,j+1])       
    
        visited.[i,j] <- true

        if Array2D.last visited then
            //pathList dijkstraMap costMap (Array2D.length1 costMap - 1) (Array2D.length2 costMap - 1) |> (fun ary -> printfn "%A" ary)
            Array2D.last dijkstraMap
        else
            //Array2D.printBool visited
            //System.Console.Clear()
            dijkstraAlgorithm costMap dijkstraMap visited
    )

let findPath (costMap: int[,]) =
    let initializer i j = 
        if i = 0 && j = 0 then
            0
        else 
            Int32.MaxValue
    let visited = Array2D.zeroCreate<bool> (Array2D.length1 costMap) (Array2D.length2 costMap)
    let distValues = Array2D.init (Array2D.length1 costMap) (Array2D.length2 costMap) initializer
    dijkstraAlgorithm costMap distValues visited

let expandMap (costMap: int[,]) : int[,] =
    let nAry = [|0..3|]
    let addNmod9plus1 n = (+) n >> (fun x -> x % 9) >> (+) 1
    let longMap = 
        nAry |> Array.fold (fun map n ->
            costMap
            |> Array2D.map (fun x -> addNmod9plus1 n x)
            |> Array2D.combineHorizontally map
        ) costMap
    let fullMap = 
        nAry |> Array.fold (fun map n ->
            longMap
            |> Array2D.map (fun x -> addNmod9plus1 n x)
            |> Array2D.combineVertically map
        ) longMap
    //Array2D.printMod10 fullMap
    fullMap
