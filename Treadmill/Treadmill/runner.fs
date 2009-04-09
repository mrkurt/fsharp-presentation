#light
namespace Mubble.Treadmill

#nowarn "57" // Yeah, yeah, Mailbox stuff is experimental
open Microsoft.FSharp.Control.Mailboxes
open System.Threading
open Mubble.Treadmill

type Func<'a> = delegate of unit -> 'a
type Command = { Action : Func<Result>; ID : string }
type Collect = { Action : Func<Benchmark>; }

type Commands = { Commands : Command seq; EndAction : Func<Result>; }

type WorkItem = Command of Command | Stop

module Runner =
    let CreateCommand a id =
        { new Command with Action = a; and ID = id }
    let CreateBenchmark a =
        { new Collect with Action = a }
        
    let Start (i : int) (t : int) (commands : Commands) (c : Collect seq) =
        let s = commands.Commands
        let logger = Logger.Create ()
    
        let metrics = Metrics.Create ()
        
        let timer = new System.Timers.Timer(1000.0)
        timer.Elapsed.Add( fun args ->
            c|> Seq.map (fun x -> x.Action.Invoke()) |> Metrics.Post metrics )
        
        timer.AutoReset <- true
        timer.Start()
    
        let handle (c : Command) = 
            let thread = System.Threading.Thread.CurrentThread.ManagedThreadId
            try
                logger.Post( Message(c.Action.Invoke(), thread ) )
            with
            | exn -> logger.Post( Exception(exn, thread) )
                
        let sem = new Semaphore(initialCount = 0, maximumCount = t)                 
        let processor (inbox : WorkItem MailboxProcessor) =  
            let rec loop() =
                async { let! msg = inbox.Receive ()
                        match msg with
                        | Command cmd ->
                            do handle(cmd)
                            return! loop ()
                        | Stop ->
                            do sem.Release() |> ignore
                            return () }
            loop()
                      
        let workers = MailboxProcessor.StartMultiple t processor
        
        let all = 
            let converted = s |> Seq.map (fun x -> Command(x))
            [1..i] |> List.map (fun x -> converted) |> Seq.concat
            
        let startTime = System.DateTime.Now
        
        MailboxProcessor.Distribute workers all
        
        printfn "Distribution took %A" (System.DateTime.Now - startTime)
        
        MailboxProcessor.SendToAll workers Stop 
        
        let rec waitFor i =
            sem.WaitOne() |> ignore
            if i > 1 then
                waitFor (i - 1)
            else ()
                
        waitFor t
        
        commands.EndAction.Invoke() |> ignore
        
        printfn "Work took %A" (System.DateTime.Now - startTime)
        
        timer.Stop()
        
        Metrics.WaitFor metrics
        Logger.WaitFor logger
        
        printfn "Complete"