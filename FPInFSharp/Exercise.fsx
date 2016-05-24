#load "Extensions.fs"
open Extensions

type Employee = { name: string; department: string; manager: Employee option }
let bob = { name = "Bob"; department = "IT"; manager = None }
let joe = { name = "Joe"; department = "IT"; manager = Some bob }
let ben = { name = "Ben"; department = "IT"; manager = Some bob }
let paul = { name = "Paul"; department = "Sales"; manager = None }
let ben' = { name = "Ben"; department = "Sales"; manager = Some paul }
let employees = [bob; joe; ben; paul; ben']

// Implement! It returns the manager name of the employees that have the specified name
let lookupManagerNames (employeeName: string) (employees: Employee list) : string list =
    failwith "Not Implemented"

Assert.isTrue (lookupManagerNames "Joe" employees = ["Bob"])
Assert.isTrue (lookupManagerNames "Ben" employees = ["Bob"; "Paul"])
Assert.isTrue (lookupManagerNames "Bob" employees = [])
Assert.isTrue (lookupManagerNames "Paul" employees = [])