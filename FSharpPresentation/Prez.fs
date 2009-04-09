(*#light
open System

let x = 7
let s = "kurt"
let pi = 3.142

let l = ['a';'b';'c';]
// char list
let digits = [0..9]
// int list

let add x y = x + y
// int -> int -> int

let add3 = add 3
// int -> int

type Car = { Model : string; Year : int }
let car = { Model = "Exige"; Year = 2008 } 
let other = { new Car
                with Model = "Pantera";
                and  Year = 1971 }

let third = { car with Year = 2007 }

let u = ()
// unit

let now () = DateTime.Now

let me = ("kurt", 6.0)
// string * float

let point3d = (0,0,0)
// int * int * int

let group x y = [x;y;]
// 'a -> 'a -> 'a list

List.map
// ('a -> 'b) -> 
// 'a list -> 
// 'b list

List.fold_left
// ('b -> 'a -> 'b) ->
// 'b ->
// 'a list ->
// 'b


let (|>) x f = f x
// 'a -> ('a -> 'b) -> 'b

let even x = x % 2 = 0
// int -> bool

let numbers = [1..20]
// int list

let evens = 
    numbers |> 
        List.filter even
// int list*)