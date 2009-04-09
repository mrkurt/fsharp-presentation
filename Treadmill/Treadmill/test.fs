#light
open System

type User = { Age : int; SignupDate : DateTime; Weight : Double; }

(* Placeholder function to retrieve a posted value *)
let post key = key
    
let parseOrDefault f v = f v |> snd
let parsePosted f key = parseOrDefault f (post key)

let u = 
    { 
        Age = post "age" |> parseOrDefault Int32.TryParse; 
        SignupDate = post "signupDate" |> parseOrDefault DateTime.TryParse;
        Weight = Double.TryParse(post "weight") |> snd;
    }
(*namespace Mubble.Treadmill
open Mubble.Treadmill

module Test = 
    let zero = 1 - 1
    let empty () = { new Command
                        with Action = new Func<Result>(fun () -> 
                                System.Threading.Thread.Sleep(100)
                                { Success = true; Message = "Did nothing!" }); 
                        and ID = "Some nothing task" }
                        
    let emptyFast = { new Command
                        with Action = new Func<Result>(fun () -> {Success = true; Message = "Did nothing, fast!"})
                        and ID = "Some fast nothing" }
    
    let metric () = Runner.CreateBenchmark (new Func<Benchmark>(fun () -> Metrics.CreateResult "Nothing!" 1.0 System.DateTime.Now))
    
    let ex () = { new Command
                        with Action = new Func<Result>(fun () ->
                                        let blah = 100 / zero
                                        System.Threading.Thread.Sleep(100)
                                        Logger.CreateResult false "This shouldn't have happened")
                        and ID = "Divide by Zero!" }

    let tasks = [ empty(); ]
    
    let metrics = [ metric (); ]

    Runner.Start 1000 8 (Seq.of_list tasks) (Seq.of_list metrics)*)
    
    //let tuple = Int32.TryParse("12345")