using System;
using System.Runtime.CompilerServices;
using EeveeTools.Servers.WebSockets;

namespace KawashimaBattleRoyaleServer {
    public class Server : WebSocketServer {
        public Server(string location, Type clientHandlerType) : base(location, clientHandlerType) {
            
        }

        public string Location => this.Server.Location;

        public int Port => this.Server.Port;
    }
}
