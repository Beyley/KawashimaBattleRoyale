using System;
using EeveeTools.Servers.WebSockets;
using KawashimaBattleRoyaleCommon;
using KawashimaBattleRoyaleCommon.Packets;
using KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima;
using KawashimaBattleRoyaleCommon.Packets.Learner;

namespace KawashimaBattleRoyaleServer {
    public class Client : WebSocketHandler {
        public string Username = null;
        public int UserId = -1;

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

            if (Username == null && tempPacket.PacketType != PacketType.LEARNER_LOGIN) {
                this.Connection.Close();
                return;
            }
            
            switch (tempPacket.PacketType) {
                case PacketType.LEARNER_LOGIN: {
                    LearnerLoginPacket packet = new();
                    packet.Deserialize(clientData);

                    this.Username = packet.username;
                    if (!CheckPassword(packet.password)) {
                        this.Connection.Close();
                        return;
                    }

                    Server.NotifyClientAboutAll(this);
                    Server.Players.Add(this);
                    Server.NotifyAllAboutLogin(this);
                    
                    break;
                }
                case PacketType.LEARNER_LOGOUT: {
                    Logout();
                    
                    break;
                }
                default: {
                    Console.WriteLine($"Unhandled packet {tempPacket.PacketType}!");
                    
                    break;
                }
            }
        }

        public void NotifyAboutLogout(Client client) {
            DrKawashimaLearnerLogoutPacket packet = new DrKawashimaLearnerLogoutPacket(client.UserId);

            this.Connection.Send(packet.Serialize());
        }

        public void NotifyAboutLogin(Client client) {
            DrKawashimaLearnerLoginPacket packet = new DrKawashimaLearnerLoginPacket(client.Username, client.UserId);
        }
        
        public void Logout() {
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
