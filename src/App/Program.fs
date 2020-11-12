open System.IO
open System.IO.Compression
open Newtonsoft.Json
open System.Text
open PBIDataModelMetadata

type ColumnData<'T> = {
    column: Column
    values: list<'T>
}

let ExtractModel sourcefile =
    let archive = ZipFile.Open(sourcefile, ZipArchiveMode.Read)
    let sr = new StreamReader(archive.GetEntry("DataModelSchema").Open (), Encoding.Unicode)
    JsonConvert.DeserializeObject<DataModelSchema>(sr.ReadToEnd())

[<EntryPoint>]
let main argv =
    printfn "Loading.."

    for arg in argv do
        let value = ExtractModel arg
        for table in value.model.tables do
            printfn "Table %s" (table.name)

    0 // return an integer exit code