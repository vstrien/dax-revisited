module ModelServer

open PbitReader

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

    SocketServer.startListening(51234)

    0 // return an integer exit code