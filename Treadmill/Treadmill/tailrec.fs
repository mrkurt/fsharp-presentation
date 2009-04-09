#light

let rec print i =
    if i > 0 then
        printfn "%d calls" i
        print (i - 1)
    else
        ()
               
print 500