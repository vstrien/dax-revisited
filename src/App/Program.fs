open System.IO
open System.IO.Compression
open Newtonsoft.Json
open System.Text
open PBIDataModelMetadata
open SocketServer
open FSharp.Data
open TabularModel

type ColumnData<'T> = {
    column: Column
    values: list<'T>
}

type DataTable = {
    columns: list<ColumnData<obj>>
}

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

[<EntryPoint>]
let main argv =

    let pbitfile = argv.[0]
    let value = ExtractModel pbitfile
    let sourcefolder = argv.[1]

    let all_data = [
        for table in value.model.tables do
            printfn "Loading data for %s" table.name
            LoadData (sourcefolder, table)
    ]

    //SocketServer.startListening(51234)

    0 // return an integer exit code