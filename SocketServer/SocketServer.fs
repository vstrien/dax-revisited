module SocketServer

open System
open System.Net
open System.Net.Sockets
open System.Threading.Tasks

let writeToSocket(socket:Socket) = 
    let stream = new NetworkStream(socket)
    let bytes = "Hello World\n"B
    stream.Write(bytes, 0, bytes.Length)
    stream.Dispose()
    socket.Dispose()

let startListening port =
    let local = IPAddress.Parse "127.0.0.1"
    let listener = new TcpListener(localaddr=local, port=port)
    listener.Start()
    printfn "%i is the port" port

    while true do
        Console.WriteLine "Waiting for connection..."
        listener.Server.Accept() |> writeToSocket