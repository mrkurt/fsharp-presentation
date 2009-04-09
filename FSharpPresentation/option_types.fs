#light
type name = string
type number = int
type date = System.DateTime
type meeting =
    | Personal of name   * date
    | Phone    of number * date
    
let review = Personal("Jasmine",System.DateTime.Now)
let call = Phone(8675309, System.DateTime.Now)

let what_to_do (m : meeting) =
    match m with
    | Personal(name,date) ->
        printfn "Meeting with %s at %A" name date
    | Phone(phone, date) ->
        printfn "Call %A at %A" phone date
