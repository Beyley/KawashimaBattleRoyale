using System;
using EeveeTools.Servers.WebSockets;
using KawashimaBattleRoyaleCommon;
using KawashimaBattleRoyaleCommon.Packets;
using KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima;
using KawashimaBattleRoyaleCommon.Packets.Learner;

namespace KawashimaBattleRoyaleServer {
    public class Client : WebSocketHandler {
        public Player player = new Player("", -1, false);

        protected override void OnConnectionOpen() {
            Console.WriteLine("Client Connected!");
        }
        
        protected override void OnBinaryData(byte[] data) {
            // throw new NotImplementedException();
        }
        
        protected override void OnConnectionClose() {
            Logout();
        }
        
        protected override void OnConnectionError(Exception error) {
            Logout();
        }
        
        protected override void OnMessage(string clientData) {
            Packet tempPacket = new BlankPacket();
            tempPacket.Deserialize(clientData);

            if (player.Username == null && tempPacket.PacketType != PacketType.LEARNER_LOGIN) {
                this.Connection.Close();
                return;
            }
            
            switch (tempPacket.PacketType) {
                case PacketType.LEARNER_LOGIN: {
                    LearnerLoginPacket packet = new(null, null);
                    packet.Deserialize(clientData);
                    // Console.WriteLine($"{packet.username} is logging in!");

                    this.player.Username = packet.username;
                    if (!CheckPassword(packet.password)) {
                        this.Connection.Close();
                        return;
                    }

                    Server.NotifyClientAboutAll(this);
                    Server.Players.Add(this);
                    Server.NotifyAllAboutLogin(this);
                    Console.WriteLine($"{packet.username} is logged in!");

                    break;
                }
                case PacketType.LEARNER_LOGOUT: {
                    Logout();
                    
                    break;
                }
                case PacketType.LEARNER_JOIN_GAME: {
                    this.player.Ingame = true;

                    Server.NotifyAllAboutClientJoin(this);
                    SendGameData();
                    Server.IngamePlayers.Add(this);
                    
                    break;
                }
                case PacketType.LEARNER_LEAVE_GAME: {
                    this.player.Ingame = false;
                    
                    Server.NotifyAllAboutClientLeave(this);
                    Server.IngamePlayers.Remove(this);
                    
                    break;
                }
                default: {
                    Console.WriteLine($"Unhandled packet {tempPacket.PacketType}!");
                    
                    break;
                }
            }
        }

        public void NotifyAboutLogout(Client client) {
            DrKawashimaLearnerLogoutPacket packet = new DrKawashimaLearnerLogoutPacket(client.player.UserId);

            this.Connection.Send(packet.Serialize());
        }

        public void NotifyAboutLogin(Client client) {
            DrKawashimaLearnerLoginPacket packet = new DrKawashimaLearnerLoginPacket(client.player.Username, client.player.UserId, client.player.Ingame);

            this.Connection.Send(packet.Serialize());
        }
        
        public void NotifyAboutClientJoin(Client client) {
            DrKawashimaLearnerJoinPacket packet = new DrKawashimaLearnerJoinPacket(client.player.UserId);

            this.Connection.Send(packet.Serialize());
        }
        
        public void NotifyAboutClientLeave(Client client) {
            DrKawashimaLearnerLeavePacket packet = new DrKawashimaLearnerLeavePacket(client.player.UserId);

            this.Connection.Send(packet.Serialize());
        }

        public void SendGameData() {
            //TODO IMPLEMENT THIS
        }
        
        public void Logout() {
            Console.WriteLine($"{this.player.Username} is logging out!");
            Server.Players.Remove(this);

            foreach (Client client in Server.Players) {
                client.NotifyAboutLogout(this);
            }
        }
        
        public bool CheckPassword(string password) {
            //TODO IMPLEMENT THIS
            return true;
        }
    }
}
