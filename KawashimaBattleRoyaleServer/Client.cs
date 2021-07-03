using System;
using EeveeTools.Servers.WebSockets;

namespace KawashimaBattleRoyaleServer {
    public class Client : WebSocketHandler {
        protected override void OnConnectionOpen() {
            // Console.WriteLine("New connection!");
        }
        protected override void OnBinaryData(byte[] data) {
            // throw new NotImplementedException();
        }
        protected override void OnConnectionClose() {
            // Console.WriteLine("Connection Closed!");
        }
        protected override void OnConnectionError(Exception error) {
            // Console.WriteLine("Error while connecting!");
        }
        protected override void OnMessage(string message) {
            // Console.WriteLine($"Message Recieved: {message}");
            // this.Connection.Send("ACK!");
        }
    }
}
