#light
namespace Mubble.Treadmill

#nowarn "57" // Yeah, yeah, Mailbox stuff is experimental
open Microsoft.FSharp.Control.Mailboxes

module List = 
    let round_robin l a f = 
        let l2 = Array.of_list l
        a |> Seq.iteri (fun i e -> f l2.[i % l2.Length] e)
        
module MailboxProcessor =
    
    let SendToAll (l : 'a MailboxProcessor list) m = l |> List.iter (fun mb -> mb.Post(m))
    
    let Distribute (mb : 'a MailboxProcessor list) (s : 'a seq) = 
        List.round_robin mb s (fun mb m -> mb.Post(m))
        
    let StartMultiple i f = [1..i] |> List.map (fun i -> MailboxProcessor.Start(f))