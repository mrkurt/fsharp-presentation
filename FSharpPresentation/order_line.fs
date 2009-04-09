#light

type OrderLine(name:string, quantity:int, price:float) =
    let mutable currName = name
    let mutable currQuantity = quantity
    let mutable currPrice = price
    new (name, price) = OrderLine(name, 1, price)
    member x.Name 
        with get() = currName
        and set name = currName <- name
    member x.SubTotal with get() = (Float.of_int quantity) * price
    member x.OneMore() = 
        currQuantity <- currQuantity + 1
        currQuantity