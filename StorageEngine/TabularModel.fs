(* 
 * Flexible datatypes to support the Tabular Model
 * 
 * Thanks to https://github.com/vdeurzen for pointing out type elimination.
 *
 * Heavily borrowed from Owl's DataFrames in OCaml: 
 * https://github.com/owlbarn/owl/blob/master/src/base/misc/owl_dataframe.ml 
 * 
 *)
module TabularModel

open ModelMetadata

exception Unsupported of string

type ColumnData<'T> = {
    column: Column
    values: list<'T>
}

type DataTable = {
    columns: list<ColumnData<obj>>
}

type elt = 
    | Bool of bool
    | Int64 of int64
    | Double of double
    | String of string
    | DateTime of System.DateTime
    | Any

type column = 
    | Bool_column of bool array
    | Int64_column of int64 array
    | Double_column of double array
    | String_column of string array
    | DateTime_column of System.DateTime array
    | Any_column

let unpack_bool = function
    | Bool x -> x
    | _      -> raise(Unsupported("Not bool"))

let unpack_int64 = function
    | Int64 x -> x
    | _      -> raise(Unsupported("Not int64"))

let unpack_double = function
    | Double x -> x
    | _      -> raise(Unsupported("Not double"))

let unpack_string = function
    | String x -> x
    | _      -> raise(Unsupported("Not string"))

let unpack_DateTime = function
    | DateTime x -> x
    | _      -> raise(Unsupported("Not DateTime"))

let pack_bool x = Bool x

let pack_double x = Double x

let pack_int64 x = Int64 x

let pack_string x = String x

let pack_DateTime x = DateTime x

let init_column n = function
  | Bool a   -> Bool_column ([| for i in 1 .. n -> a|])
  | Int64 a    -> Int64_column ([| for i in 1 .. n -> a|])
  | Double a  -> Double_column ([| for i in 1 .. n -> a|])
  | String a -> String_column ([| for i in 1 .. n -> a|])
  | DateTime a -> DateTime_column ([| for i in 1 .. n -> a|])
  | Any      -> Any_column

type table = {
    data : column array
}