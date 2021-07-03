using System;
using EeveeTools.Servers.WebSockets;

namespace KawashimaBattleRoyaleServer {
    public class Client : WebSocketHandler {
        protected override void OnConnectionOpen() {
            throw new NotImplementedException();
        }
        protected override void OnBinaryData(byte[] data) {
            throw new NotImplementedException();
        }
        protected override void OnConnectionClose() {
            throw new NotImplementedException();
        }
        protected override void OnConnectionError(Exception error) {
            throw new NotImplementedException();
        }
        protected override void OnMessage(string message) {
            throw new NotImplementedException();
        }
    }
}
