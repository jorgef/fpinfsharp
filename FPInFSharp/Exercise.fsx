#load "Extensions.fs"
open Extensions

type Employee = { name: string; department: string; manager: Employee option }
let bob = { name = "Bob"; department = "IT"; manager = None }
let joe = { name = "Joe"; department = "IT"; manager = Some bob }
let ben = { name = "Ben"; department = "IT"; manager = Some bob }
let paul = { name = "Paul"; department = "Sales"; manager = None }
let ben' = { name = "Ben"; department = "Sales"; manager = Some paul }
let employees = [bob; joe; ben; paul; ben']

// Implement
let lookupManagerName (employeeName: string) (defaultName: string) (employees: Employee list) : string list =
    failwith ""

Assert.isTrue (lookupManagerName "Joe" "N/A" employees = ["Bob"])
Assert.isTrue (lookupManagerName "Ben" "N/A" employees = ["Bob"; "Paul"])
Assert.isTrue (lookupManagerName "Bob" "N/A" employees = ["N/A"])
Assert.isTrue (lookupManagerName "Paul" "N/A" employees = ["N/A"])