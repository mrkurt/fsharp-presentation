#light
namespace Mubble.Treadmill

#nowarn "57" // Yeah, yeah, Mailbox stuff is experimental
open Microsoft.FSharp.Control.Mailboxes
open Mubble.Treadmill

type Result = { Success : bool; Message : string }
type Benchmark = { Name : string; Value : float; Time : System.DateTime }

type BenchmarkMessage = Value of Benchmark | Log of IChannel<int>
type ResultMessage = Message of (Result * int) | Exception of (exn * int) | Log of IChannel<int>

module Logger = 
    let CreateResult s m = { new Result with Success = s and Message = m }
    let Create () = MailboxProcessor.Start(fun inbox ->
        let file = System.IO.File.CreateText("raw_results.txt")
        file.WriteLine( (sprintf "#Started at %A" System.DateTime.Now) )
        file.WriteLine("Iteration, Status, Thread, Message")
        let rec run i = 
            async { let! r = inbox.Receive()
                    match r with
                    | Message (r, thread) ->
                        do file.WriteLine( (sprintf "%d, sucess, %d, \"%s\"" i thread r.Message) )
                        return! run (i + 1) 
                    | Exception (ex, thread) ->
                        //do printfn "%d An exception occurred on thread %d - %s" i thread ex.Message
                        do file.WriteLine( (sprintf "%d, fail, %d, \"%s\"" i thread ex.Message) )
                        return! run (i + 1)
                    | Log replyChannel -> 
                        do file.WriteLine( (sprintf "#Ended at %A" System.DateTime.Now) )
                        do file.Close()
                        do replyChannel.Post(1)
                        return () }
        run 1)
        
    let WaitFor (logger : ResultMessage MailboxProcessor) = logger.PostSync( fun x -> Log(x) ) |> ignore
    
module Metrics = 
    let CreateResult n v t = 
        { new Benchmark with Name = n; and Value = v; and Time = t }
        
    let Create () = MailboxProcessor.Start(fun inbox ->
        let file = System.IO.File.CreateText("metrics.txt")
        file.WriteLine( (sprintf "#Started at %A" System.DateTime.Now) )
        file.WriteLine("Name, Value, Time")
        let rec run i time = 
            async { let! msg = inbox.Receive()
                    match msg with
                    | Value v ->
                        do file.WriteLine( (sprintf "%s, %f, %A" v.Name v.Value (v.Time - time)) )
                        return! run (i + 1) time
                    | BenchmarkMessage.Log replyChannel ->
                        do file.WriteLine( (sprintf "#Ended at %A" System.DateTime.Now) )
                        do file.Close()
                        do replyChannel.Post(i)
                        return () }
        run 0 System.DateTime.Now)
        
    let Post (mailbox : BenchmarkMessage MailboxProcessor) values = 
        Seq.iter (fun v -> mailbox.Post( Value(v) )) values
        
    let WaitFor (metrics : BenchmarkMessage MailboxProcessor) = metrics.PostSync(fun x -> BenchmarkMessage.Log(x) ) |> ignore
                    