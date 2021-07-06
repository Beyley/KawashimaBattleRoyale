using System;
using System.Collections.Generic;
using EeveeTools.Servers.WebSockets;

namespace KawashimaBattleRoyaleServer {
	public class Server : WebSocketServer {
		public static List<Client> Players       = new();
		public static List<Client> IngamePlayers = new();

		public static Random r = new();

		public Server(string location, Type clientHandlerType) : base(location, clientHandlerType) {}

		public static void NotifyAllAboutClientJoin(Client client) {
			foreach (var currentClient in Players)
				currentClient.NotifyAboutClientJoin(client);
		}

		public static void NotifyAllAboutClientLeave(Client client) {
			foreach (var currentClient in Players)
				currentClient.NotifyAboutClientLeave(client);
		}

		public static void NotifyAllAboutLogin(Client client) {
			foreach (var currentClient in Players)
				currentClient.NotifyAboutLogin(client);
		}

		public static void NotifyClientAboutAll(Client client) {
			foreach (var currentClient in Players)
				client.NotifyAboutLogin(currentClient);
		}
	}
}
