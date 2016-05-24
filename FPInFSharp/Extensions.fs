namespace Extensions

open System

module Assert =
    let isTrue condition = if condition then "Passed" else "Failed"

module Option =

    /// An empty nullable value of a type designated by use.
    let emptyNullable = new Nullable<_>()

    /// Converts an option to a corresponding nullable.
    let inline toNullable opt = Option.fold (fun _ v -> new Nullable<_>(v)) emptyNullable opt

    let inline asNullable value = new Nullable<_>(value)

    let ofNullable (value:Nullable<_>) =
        if value.HasValue then Some value.Value
        else None

    /// Maps null references returned as proper (non-nullable) F# values by Linq Query Expressions
    /// http://stackoverflow.com/a/26008852
    let ofNull (value:'a) =
        if obj.ReferenceEquals(value, null) then None
        else Some value

    /// Given a default value and an option, returns the option value if there else the default value.
    let inline isNull defaultValue = function Some v -> v | None -> defaultValue

    /// Given a default value and an option, returns the option value if there else the default value.
    let inline getValueOr defaultValue = function Some v -> v | None -> defaultValue

    /// Given a default value and an option, returns the option value if there else the default value.
    let inline isNullLazy defaultValue = function Some v -> v | None -> defaultValue()

    /// Gets the Some value or Unchecked.defaultof<'a>.
    let inline getValueOrDefault (o:'a option) = isNull (Unchecked.defaultof<'a>) o

    let inline (|?) value defaultValue = isNull value defaultValue

    let inline join (a:'a option option) : 'a option = a |> Option.bind id

    /// Merges two options into one by returning the first if it is Some otherwise returning the second.
    /// This operation is not commutative but is associative.
    let merge (a:'a option) (b:'a option) : 'a option =
        match a with
        | Some _ -> a
        | None -> b

    /// Folds an option by applying f to Some otherwise returning the default value.
    let inline foldOr (f:'a -> 'b) (defaultValue:'b) = function Some a -> f a | None -> defaultValue

    /// Folds an option by applying f to Some otherwise returning the default value.
    let inline foldOrLazy (f:'a -> 'b) (defaultValue:unit -> 'b) = function Some a -> f a | None -> defaultValue()

    /// Return Some (f ()) if f doesn't raise. None otherwise
    let inline tryWith (f : unit -> 'a) = try () |> f|> Some with _ -> None

    /// Converts a pair of a value and an optional value into an optional value containing a pair.
    [<Obsolete("use liftFst instead")>]
    let strength = function
        | a, Some b -> Some (a,b)
        | a, None -> None

    /// Lifts the first element of a pair into an option based on the value of the second element option.
    let liftFst = function
        | a, Some b -> Some (a,b)
        | _, None -> None

    /// Lifts the second element of a pair into an option based on the value of the first element option.
    let liftSnd = function
        | Some a, b -> Some (a,b)
        | None, _ -> None

    /// Applies a function to values contained in both options when both are Some.
    let map2 f a b =
        match a, b with
        | Some a, Some b -> f a b |> Some
        | _ -> None

    /// When "Some a" returns "Choice1Of2 a" and "Choice2Of2 e" otherwise.
    let successOr (e:'e) (o:'a option) : Choice<'a, 'e> =
        match o with
        | Some a -> Choice1Of2 a
        | None -> Choice2Of2 e

    // ------------------------------------------------------------------------------------------------------------------

[<AutoOpen>]
module ChoiceTypes =
    /// Active pattern for matching a Choice<'a, 'b> with Choice1Of2 = Success and Choice2Of2 = Failure
    let (|Success|Failure|) = function
        | Choice1Of2 a -> Success a
        | Choice2Of2 b -> Failure b

    /// Indicates success as Choice1Of2
    let Success = Choice1Of2

    /// Indicates failure as Choice2Of2
    let Failure = Choice2Of2


module Choice =

    let returnM : 'a -> Choice<'a, 'b> = Choice1Of2

    /// Maps over the left result type.
    let mapl (f:'a -> 'b) = function
      | Choice1Of2 a -> f a |> Choice1Of2
      | Choice2Of2 e -> Choice2Of2 e

    /// Maps over the right result type.
    let mapr (f:'b -> 'c) = function
      | Choice1Of2 a -> Choice1Of2 a
      | Choice2Of2 e -> f e |> Choice2Of2

    /// Maps over the left result type.
    [<System.Obsolete("Use Choice.mapl")>]
    let map (f:'a -> 'b) = function
      | Choice1Of2 a -> f a |> Choice1Of2
      | Choice2Of2 e -> Choice2Of2 e

    /// Maps over the left or the right result type.
    let bimap (f1:'a -> 'c) (f2:'b -> 'd) = function
      | Choice1Of2 x -> Choice1Of2 (f1 x)
      | Choice2Of2 x -> Choice2Of2 (f2 x)

    /// Folds a choice pair with functions for each case.
    let fold (f1:'a -> 'c) (f2:'b -> 'c) = function
      | Choice1Of2 x -> f1 x
      | Choice2Of2 x -> f2 x

    /// Extracts the value from a choice with the same type on the left as the right.
    /// (Also known as the codiagonal morphism).
    let codiag<'a> : Choice<'a, 'a> -> 'a =
      fold id id

    /// Binds a function to a choice where the right case represents failure to be propagated.
    let bind (f:'a -> Choice<'b, 'e>) = function
      | Choice1Of2 a -> f a
      | Choice2Of2 e -> Choice2Of2 e

    /// evaluate f () and return either the result of the evaluation or the exception
    let tryWith f = try Choice1Of2 (f ()) with exn -> Choice2Of2 exn

    /// Return Some v if either is Success v. Otherwise return None.
    let toOption = function
        | Choice1Of2 a -> Some a
        | Choice2Of2 _ -> None

    let ofOption error = Option.foldOr Choice1Of2 (Choice2Of2 error)

    let filter (f: 'a -> bool) error = function
        | Choice1Of2 a ->
            if f a then Choice1Of2 a
            else Choice2Of2 error
        | Choice2Of2 error -> Choice2Of2 error

    /// Merges two choices which can potentially contain errors.
    /// When both choice values are errors, they are concatenated using ';'.
    let mergeErrs = function
      | Choice1Of2 (), Choice1Of2 () -> Choice1Of2 ()
      | Choice2Of2 e, Choice1Of2 _   -> Choice2Of2 e
      | Choice1Of2 _, Choice2Of2 e   -> Choice2Of2 e
      | Choice2Of2 e1, Choice2Of2 e2 -> Choice2Of2 (String.concat ";" [e1;e2])
