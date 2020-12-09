module AdventOfCodeFSharpFoo
   
open System
open AdventOfCodeFSharp
open AdventOfCodeFSharp.Day08

[<EntryPoint>]
let main argv =
    let (part1Val,_) = part1
    let part2Val = Seq.head part2
    
    printfn "Answer to Day 8 Part 1: %d" part1Val
    printfn "Answer to Day 8 Part 2: %d" part2Val
    0 // return an integer exit code