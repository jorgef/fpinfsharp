(*

Chapter 3 - Functional data structures

- Immutable Lists
- Trees

*)


(*

Single Linked List:

           head      ________tail__________  
            ---      ---      ---      --- 
list1:     | 1 | -> | 2 | -> | 3 | -> | 4 |
            ---      ---      ---      --- 
             ^
             |
        ---  |
list2: | 0 |-
        --- 
        head

*)

type List<'a> =
    | Nil
    | Cons of 'a * List<'a> 

let list1 = Cons(1, Cons(2, Cons(3, Cons(4, Nil))))
let list2 = Cons(0, list1)

let list1' = [1; 2; 3; 4]
let list2' = 0 :: list1'













// Filer: ('a -> bool) -> 'a list -> 'a list

[1; 2; 3; 4] |> List.filter (fun i -> i > 2)  // [3; 4]
























// Map: ('a -> 'b) -> 'a list -> 'b list

[1; 2; 3; 4] |> List.map (fun i -> i + 1)  // [2; 3; 4; 5]

[1; 2; 3; 4] |> List.map (fun i -> i.ToString ())  // ["1"; "2"; "3"; "4"]





















// Collect / Bind / FlatMap: ('a -> 'b list) -> 'a list -> 'b list

[1; 2; 3; 4] |> List.collect (fun i -> [-i; i])  // [-1; 1; -2; 2; -3; 3; -4; 4]
 
[1; 2; 3; 4] |> List.collect (fun i -> if i > 2 then [i] else [])  // [3; 4]

[1; 2; 3; 4] |> List.collect (fun i -> [i.ToString ()])  //  ["1"; "2"; "3"; "4"]
















// Fold: ('a -> 'b -> 'a) -> 'a -> 'b list -> 'a

[1; 2; 3; 4] |> List.fold (fun s i -> s + i) 0 // 10

[1; 2; 3; 4] |> List.fold (fun s i -> s + i.ToString ()) "" // "1234"

[1; 2; 3; 4] |> List.fold (fun s i -> i :: s) [] // [4; 3; 2; 1]

[1; 2; 3; 4] |> List.fold (fun s i -> if i > 2 then i :: s else s) [] // [4; 3]

[1; 2; 3; 4] |> List.fold (fun s i -> -i :: i :: s ) [] // [-4; 4; -3; 3; -2; 2; -1; 1]