#load "Extensions.fs"
open Extensions


(*
    Chapter 4 - Handling errors without exceptions

    - Exceptions
    - Options
    - Choices
*)


// Total Functions - Example Length: int list -> int


[1; 2; 3; 4] |> List.length // 4

[] |> List.length // 0







// Partial Functions - Example Min: int list -> int

[1; 2; 3; 4] |> List.min // 1

([]: int list) |> List.min // ArgumentException: The input sequence was empty















(*
    Option

    type Option<'a> =
        | None
        | Some of 'a
*)

let totalMin (ls: int list) =
    match ls with
    | [] -> None
    | ls -> ls |> List.min |> Some

([]: int list) |> totalMin // None

[1; 2; 3; 4] |> totalMin // Some 1















// Map ('a -> 'b) -> 'a option -> 'b option

// See Option as a List:  None ~ [] / Some 1 ~ [1]

(Some 1) |> Option.map (fun i -> i + 1) // Some 2

None |> Option.map (fun i -> i + 1) // None


// Map Example:

type Employee = { name: string; department: string; manager: Employee option }
let manager = { name = "Bob"; department = "IT"; manager = None }
let employee = { name = "Joe"; department = "IT"; manager = Some manager }
let lookupByName (name: string) = if name = "Joe" then Some employee else None
let getDepartment employee = employee.department

"Joe"
|> lookupByName
|> getDepartment // The type 'Employee option' does not match the type 'Employee'

"Joe"
|> lookupByName
|> Option.map getDepartment  // Some "IT"

"Ben"
|> lookupByName
|> Option.map getDepartment // None


// Using Map to Lift a Function

let optionalGetDepartment = Option.map getDepartment // int option -> bool option










// Bind / FlatMap: ('a -> 'b option) -> 'a option -> 'b option

let getManager employee = employee.manager

"Joe"
|> lookupByName
|> Option.map getManager  // Some of Some of Employee


"Joe"
|> lookupByName
|> Option.bind getManager  // Some of Employee









// Fold: ('a -> 'b -> 'a) -> 'a -> 'b option -> 'a

"Ben"
|> lookupByName
|> Option.map getDepartment
|> Option.fold (fun _ d -> d) "N/A"  // "N/A"

"Joe"
|> lookupByName
|> Option.map getDepartment
|> Option.fold (fun _ d -> d) "N/A" // "IT"


// Marvel's Alternative - GetValueOr: 'a -> 'a option -> 'a

"Ben"
|> lookupByName
|> Option.map getDepartment
|> Option.getValueOr "N/A" // "IT"









// Filter: ('a -> bool) -> 'a option -> 'a option

"Joe"
|> lookupByName
|> Option.filter (fun e -> e.manager = None) // None















(*
    Choice

    type Choice<'a, 'b> =
        | Choice1Of2 of 'a  // Success of 'a
        | Choice2Of2 of 'b  // Failure of 'b
*)


let lookupByName (name: string) = if name = "Joe" then Success employee else Failure "Employee not found"

"Joe"
|> lookupByName
|> Choice.mapl getDepartment

"Ben"
|> lookupByName
|> Choice.mapl getDepartment