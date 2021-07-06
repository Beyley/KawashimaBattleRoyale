using System;
using System.Collections.Generic;
using EeveeTools.Servers.WebSockets;
using KawashimaBattleRoyaleCommon;
using KawashimaBattleRoyaleCommon.Packets;
using KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima;
using KawashimaBattleRoyaleCommon.Packets.Learner;

namespace KawashimaBattleRoyaleServer {
	public class Client : WebSocketHandler {
		public Player player = new("", -1, false);

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

		protected override void OnMessage(string clientData) {
			Packet tempPacket = new BlankPacket();
			tempPacket.Deserialize(clientData);

			if (this.player.Username == null && tempPacket.PacketType != PacketType.LEARNER_LOGIN) {
				this.Connection.Close();
				return;
			}

			switch (tempPacket.PacketType) {
				case PacketType.LEARNER_LOGIN: {
					LearnerLoginPacket packet = new(null, null);
					packet.Deserialize(clientData);
					// Console.WriteLine($"{packet.username} is logging in!");

					this.player.Username = packet.username;
					if (!this.CheckPassword(packet.password)) {
						this.Connection.Close();
						return;
					}

					Server.NotifyClientAboutAll(this);
					Server.Players.Add(this);
					Server.NotifyAllAboutLogin(this);
					Console.WriteLine($"{packet.username} has logged in!");

					break;
				}
				case PacketType.LEARNER_LOGOUT: {
					this.Logout();

					break;
				}
				case PacketType.LEARNER_JOIN_GAME: {
					this.player.Ingame = true;

					Server.NotifyAllAboutClientJoin(this);
					this.SendGameData();
					Server.IngamePlayers.Add(this);

					break;
				}
				case PacketType.LEARNER_LEAVE_GAME: {
					this.player.Ingame = false;

					Server.NotifyAllAboutClientLeave(this);
					Server.IngamePlayers.Remove(this);

					break;
				}
				case PacketType.LEARNER_SEND_QUESTION: {
					LearnerSendQuestionPacket packet = new(0);
					packet.Deserialize(clientData);

					if (Server.IngamePlayers.Count <= 1) return;

					List<Client> temporaryList = new(Server.IngamePlayers);
					temporaryList.Remove(this);
					temporaryList = new(temporaryList);

					var client = temporaryList[Server.r.Next(temporaryList.Count)];

					client.SendQuestion(packet.type);

					break;
				}
				default: {
					Console.WriteLine($"Unhandled packet {tempPacket.PacketType}!");

					break;
				}
			}
		}

		public void SendQuestion(ProblemTypes type) {
			DrKawashimaSendQuestionPacket packet = new(type);

			this.Connection.Send(packet.Serialize());
		}

		public void NotifyAboutLogout(Client client) {
			var packet = new DrKawashimaLearnerLogoutPacket(client.player.UserId);

			this.Connection.Send(packet.Serialize());
		}

		public void NotifyAboutLogin(Client client) {
			var packet = new DrKawashimaLearnerLoginPacket(client.player.Username, client.player.UserId, client.player.Ingame);

			this.Connection.Send(packet.Serialize());
		}

		public void NotifyAboutClientJoin(Client client) {
			var packet = new DrKawashimaLearnerJoinPacket(client.player.UserId);

			this.Connection.Send(packet.Serialize());
		}

		public void NotifyAboutClientLeave(Client client) {
			var packet = new DrKawashimaLearnerLeavePacket(client.player.UserId);

			this.Connection.Send(packet.Serialize());
		}

		public void SendGameData() {
			//TODO IMPLEMENT THIS
		}

		public void Logout() {
			Console.WriteLine($"{this.player.Username} has logged out!");

			Server.Players.Remove(this);
			Server.IngamePlayers.Remove(this);

			foreach (var client in Server.Players)
				client.NotifyAboutLogout(this);
		}

		public bool CheckPassword(string password) {
			//TODO IMPLEMENT THIS
			return true;
		}
	}
}
