#load "Extensions.fs"
open Extensions

type Employee = { name: string; department: string; manager: Employee option }
let bob = { name = "Bob"; department = "IT"; manager = None }
let joe = { name = "Joe"; department = "IT"; manager = Some bob }
let ben = { name = "Ben"; department = "IT"; manager = Some bob }
let paul = { name = "Paul"; department = "Sales"; manager = None }
let ben' = { name = "Ben"; department = "Sales"; manager = Some paul }
let employees = [bob; joe; ben; paul; ben']

let lookupManagerNames1 (employeeName: string) (employees: Employee list) : string list =
    employees |> List.collect (fun i -> if i.name = employeeName then i.manager |> Option.fold (fun _ m -> [m.name]) [] else [])

let lookupManagerNames2 (employeeName: string) (employees: Employee list) : string list =
    employees
    |> List.filter (fun e -> e.name = employeeName)
    |> List.collect (fun e -> e.manager |> Option.fold (fun _ m -> [m.name]) [])

let lookupManagerNames3 (employeeName: string) (employees: Employee list) : string list =
    employees
    |> List.fold (fun l e -> if e.name = employeeName then e.manager |> Option.fold (fun _ m -> m.name :: l) l else l) []
    |> List.rev

let lookupManagerNames4 (employeeName: string) (employees: Employee list) : string list =
    employees
        |> List.filter (fun x -> x.name = employeeName)
        |> List.map (fun x -> x.manager |> Option.map(fun x -> x.name))
        |> Seq.choose id
        |> List.ofSeq

let lookupManagerNames5 (employeeName: string) (employees: Employee list) : string list =
    employees
       |> List.filter (fun x -> x.name = employeeName)
       |> List.map (fun x -> x.manager)
       |> List.collect Option.toList
       |> List.map (fun x -> x.name)

let lookupManagerNames6 (employeeName: string) (employees: Employee list):string list  =
    employees
    |> List.collect(fun x -> if (x.name = employeeName && (Option.isSome x.manager)) then [x.manager] else [])
    |> List.map(fun x -> Option.fold (fun _ m -> m.name) "" x)

let lookupManagerNames7 (employeeName: string) (employees: Employee list) : string list =
    employees |> List.collect (fun e-> if e.name = employeeName then [e.manager] else [])
              |> List.filter (fun x->x<>None)
              |> List.map (Option.map (fun x->x.name) >> Option.getValueOr "")

let lookupManagerNames8 (employeeName: string) (employees: Employee list) : string list =
    employees |> List.fold (fun s e -> e.manager |> Option.foldOr (fun m -> if e.name = employeeName then m.name::s else s) s) [] |> List.rev

let lookupManagerNames9 (employeeName: string) (employees: Employee list) : string list =
    employees |> List.collect (fun e -> e.manager |> Option.foldOr (fun m -> if e.name = employeeName then [m.name] else []) [])

let getManager employee = employee.manager

Assert.isTrue (lookupManagerNames1 "Joe" employees = ["Bob"])
Assert.isTrue (lookupManagerNames1 "Ben" employees = ["Bob"; "Paul"])
Assert.isTrue (lookupManagerNames1 "Bob" employees = [])
Assert.isTrue (lookupManagerNames1 "Paul" employees = [])
Assert.isTrue (lookupManagerNames2 "Joe" employees = ["Bob"])
Assert.isTrue (lookupManagerNames2 "Ben" employees = ["Bob"; "Paul"])
Assert.isTrue (lookupManagerNames2 "Bob" employees = [])
Assert.isTrue (lookupManagerNames2 "Paul" employees = [])
Assert.isTrue (lookupManagerNames3 "Joe" employees = ["Bob"])
Assert.isTrue (lookupManagerNames3 "Ben" employees = ["Bob"; "Paul"])
Assert.isTrue (lookupManagerNames3 "Bob" employees = [])
Assert.isTrue (lookupManagerNames3 "Paul" employees = [])
Assert.isTrue (lookupManagerNames4 "Joe" employees = ["Bob"])
Assert.isTrue (lookupManagerNames4 "Ben" employees = ["Bob"; "Paul"])
Assert.isTrue (lookupManagerNames4 "Bob" employees = [])
Assert.isTrue (lookupManagerNames4 "Paul" employees = [])
Assert.isTrue (lookupManagerNames5 "Joe" employees = ["Bob"])
Assert.isTrue (lookupManagerNames5 "Ben" employees = ["Bob"; "Paul"])
Assert.isTrue (lookupManagerNames5 "Bob" employees = [])
Assert.isTrue (lookupManagerNames5 "Paul" employees = [])
Assert.isTrue (lookupManagerNames6 "Joe" employees = ["Bob"])
Assert.isTrue (lookupManagerNames6 "Ben" employees = ["Bob"; "Paul"])
Assert.isTrue (lookupManagerNames6 "Bob" employees = [])
Assert.isTrue (lookupManagerNames6 "Paul" employees = [])
Assert.isTrue (lookupManagerNames7 "Joe" employees = ["Bob"])
Assert.isTrue (lookupManagerNames7 "Ben" employees = ["Bob"; "Paul"])
Assert.isTrue (lookupManagerNames7 "Bob" employees = [])
Assert.isTrue (lookupManagerNames7 "Paul" employees = [])
Assert.isTrue (lookupManagerNames8 "Joe" employees = ["Bob"])
Assert.isTrue (lookupManagerNames8 "Ben" employees = ["Bob"; "Paul"])
Assert.isTrue (lookupManagerNames8 "Bob" employees = [])
Assert.isTrue (lookupManagerNames8 "Paul" employees = [])
Assert.isTrue (lookupManagerNames9 "Joe" employees = ["Bob"])
Assert.isTrue (lookupManagerNames9 "Ben" employees = ["Bob"; "Paul"])
Assert.isTrue (lookupManagerNames9 "Bob" employees = [])
Assert.isTrue (lookupManagerNames9 "Paul" employees = [])
