open System.IO
open System.IO.Compression
open Newtonsoft.Json
open System.Text
open PBIDataModelMetadata
open FSharp.Data

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

let LoadData (sourcefolder: string, table: Table) =
    let tablecontents = 
        sprintf "%s/%s.csv" sourcefolder table.name
        |> CsvFile.Load
    
    [
        for column in table.columns do
            if tablecontents.TryGetColumnIndex(column.name) <> None then
                let columnValues = [
                    for row in tablecontents.Rows do
                        row.[column.name] // This is not great: integers are stored as strings. However, first focus is a working DAX engine - we can load data. And that's cool.
                ]
                                
                {
                    column = column
                    values = columnValues
                }
    ]

[<EntryPoint>]
let main argv =
    printfn "Loading.."

    let pbitfile = argv.[0]
    let value = ExtractModel pbitfile
    let sourcefolder = argv.[1]

    let all_data = [
        for table in value.model.tables do
            printfn "Loading data for %s" table.name
            LoadData (sourcefolder, table)
    ]

    
    0 // return an integer exit code