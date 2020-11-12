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
    let tablename = table.name
    let filepath = sprintf "/Users/vstrien/src/hobby/dax-revisited/sample-file/csv/%s.csv" tablename
    let tablecontents = CsvFile.Load(filepath)
    
    let tableColumnDataList = [
        for column in table.columns do
            printfn "Trying to get column index of %s " column.name
            if tablecontents.TryGetColumnIndex(column.name) <> None then
                printfn "Column %s present. Loading ..." column.name
                let columnIndex = tablecontents.GetColumnIndex(column.name)
                let columnValues = [
                    for row in tablecontents.Rows do
                        row.[columnIndex]
                ]
                
                printfn "%d rows loaded" columnValues.Length

                let columnData = {
                    column = column
                    values = columnValues
                }
                columnData
    ]

    tableColumnDataList
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