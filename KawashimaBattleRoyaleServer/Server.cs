using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EeveeTools.Servers.WebSockets;

namespace KawashimaBattleRoyaleServer {
    public class Server : WebSocketServer {
        public static List<Client> Players = new();

        public static void NotifyAllAboutLogin(Client client) {
            foreach (Client currentClient in Players) {
                currentClient.NotifyAboutLogin(client);
            }
        }
        
        public static void NotifyClientAboutAll(Client client) {
            foreach (Client currentClient in Players) {
                client.NotifyAboutLogin(currentClient);
            }
        }
        
        public Server(string location, Type clientHandlerType) : base(location, clientHandlerType) {
            
        }
    }
}
