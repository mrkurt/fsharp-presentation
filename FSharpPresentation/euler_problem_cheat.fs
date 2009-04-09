#light
open System

let divisibleBy divisor value = value % divisor = 0

let values = Seq.unfold (fun x -> Some(x, x + 20)) 20

let divisors = List.rev [2 .. 19]

let divisibleByAll divisors value = 
    let notDivisibleBy x = (divisibleBy x value) = false
    let exists = divisors |> List.exists notDivisibleBy
    exists = false

let finder = Seq.find (fun x -> divisibleByAll divisors x)

values |> finder |> print_any
