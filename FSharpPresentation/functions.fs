#light

let rec fib n =
    if n < 2 then
        n
    else
        (fib (n - 1)) + 
        (fib (n - 2))

let math a b =
    let x = a + b
    let x = x * a
    let x = x / b
    printfn "The number is %i" x