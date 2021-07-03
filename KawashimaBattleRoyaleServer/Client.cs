using System;
using EeveeTools.Servers.WebSockets;

namespace KawashimaBattleRoyaleServer {
    public class Client : WebSocketHandler {
        private bool isLoggedIn = false;
        private string username;
        
        protected override void OnConnectionOpen() {
            Console.WriteLine("Client Connected!");
        }
        protected override void OnBinaryData(byte[] data) {
            // throw new NotImplementedException();
        }
        protected override void OnConnectionClose() {
            
        }
        protected override void OnConnectionError(Exception error) {
            
        }
        protected override void OnMessage(string message) {
            
        }
    }
}
