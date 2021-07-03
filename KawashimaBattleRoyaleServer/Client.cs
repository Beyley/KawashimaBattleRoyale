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
            this.Logout();
        }
        protected override void OnConnectionError(Exception error) {
            this.Logout();
        }
        protected override void OnMessage(string message) {
            if (!this.isLoggedIn) {
                this.username = message;

                this.Connection.Send("logged in");
                Server.players.Add(this);
                return;
            }

            switch (message) {
                case "get gamedata": {
                    break;
                }
                case "logout": {
                    this.Logout();
                    break;
                }
            }
        }

        public void Logout() {
            Server.players.Remove(this);
        }
    }
}
