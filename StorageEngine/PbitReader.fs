module PbitReader

open System.IO
open System.IO.Compression
open Newtonsoft.Json
open System.Text
open FSharp.Data
open ModelMetadata
open TabularModel

let ExtractModel sourcefile =
    let archive = ZipFile.Open(sourcefile, ZipArchiveMode.Read)
    let sr = new StreamReader(archive.GetEntry("DataModelSchema").Open (), Encoding.Unicode)
    JsonConvert.DeserializeObject<DataModelSchema>(sr.ReadToEnd())

let StringToBool s =
    match s with
    | "True" -> true
    | "False" -> false
    | _-> failwith("Error: returns " + s)

let LoadData (sourcefolder: string, table: Table) =
    let tablecontents = 
        sprintf "%s/%s.csv" sourcefolder table.name
        |> CsvFile.Load
    
    [
        for column in table.columns do
            if tablecontents.TryGetColumnIndex(column.name) <> None then
                match column.dataType with
                | "bool" -> Bool_column([| for r in tablecontents.Rows -> StringToBool(r.[column.name]) |])
                | "int64" -> Int64_column([| for r in tablecontents.Rows -> (int64)r.[column.name] |])
                | "double" -> Double_column([| for r in tablecontents.Rows -> (double)r.[column.name] |])
                | "dateTime" -> DateTime_column([| for r in tablecontents.Rows -> System.DateTime.Parse r.[column.name] |])
                | "string" | _ -> String_column([| for r in tablecontents.Rows -> r.[column.name] |])
    ]