using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KawashimaBattleRoyaleClient.Screens;
using KawashimaBattleRoyaleCommon;
using KawashimaBattleRoyaleCommon.Packets;
using KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima;
using KawashimaBattleRoyaleCommon.Packets.Learner;
using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine.Graphics.Notifications;
using PeppyCodeEngineGL.Engine.Helpers;
using WebSocketSharp;
using CryptoHelper = EeveeTools.Helpers.CryptoHelper;

namespace KawashimaBattleRoyaleClient {
    public class OnlineHandler {
        public WebSocket Socket;

        public OnlineState State = OnlineState.OFFLINE;

        public Player Player = new Player("", -1);
        
        public bool LoggedIn {
            get {
                return State == OnlineState.LOGGED_IN || State == OnlineState.IN_GAME;
            }
        }

        public List<Player> Players = new ();
        
        public OnlineHandler(string location) {
            this.Socket = new WebSocket(location);

            this.Socket.OnMessage += OnMessage;
            this.Socket.OnClose += OnClose;
            this.Socket.OnError += OnError;
        }

        private void OnMessage(object? sender, MessageEventArgs args) {
            if (!args.IsText) return;
            
            Packet tempPacket = new BlankPacket();
            tempPacket.Deserialize(args.Data);

            switch (tempPacket.PacketType) {
                case PacketType.DRKAWASHIMA_LEARNER_LOGIN: {
                    DrKawashimaLearnerLoginPacket packet = new();
                    packet.Deserialize(args.Data);

                    if (packet.Username == this.Player.Username) {
                        this.Player = new(packet.Username, packet.UserId);
                        this.State = OnlineState.LOGGED_IN;
                        Console.WriteLine($"You are logged in!");
                    }
                    else {
                        //Check if player is in the list
                        int PlayerIndex = -1;
                        for (int i = 0; i < this.Players.Count; i++) {
                            Player player = this.Players[i];
                            if (player.UserId == packet.UserId) {
                                PlayerIndex = i;
                                
                                //Update the player to the new data
                                player.Username = packet.Username;
                            }
                        }
                        
                        //Add the player to the list if they are not there
                        if(PlayerIndex == -1)
                            this.Players.Add(new(packet.Username, packet.UserId));
                    }

                    break;
                }
            }
        }

        private void OnClose(object? sender, CloseEventArgs args) {
            NotificationManager.CreateNotification(NotificationManager.NotificationType.LeftBlob, Color.Red, $"Connection closed by server. Reason: {args.Reason}", 5000);

            OnDisconnect();
        }

        private void OnError(object? sender, ErrorEventArgs args) {
            NotificationManager.CreateNotification(NotificationManager.NotificationType.LeftBlob, Color.Red, $"Connection error occured! Message: {args.Message}", 5000);
            
            OnDisconnect();
        }

        /// <summary>
        /// Run when the client disconnects from the server
        /// </summary>
        public void OnDisconnect() {
            State = OnlineState.OFFLINE;
            
            Console.WriteLine("Disconnected!");
        }

        /// <summary>
        /// Used to log in to the server
        /// </summary>
        /// <param name="username">The clients username</param>
        /// <param name="password">The clients password</param>
        public void Login(string username, string password) {
            string hashedPassword = CryptoHelper.HashSha256(password);

            this.Player.Username = username;
            
            LearnerLoginPacket packet = new(username, hashedPassword);
            
            this.SendString(packet.Serialize());

            this.State = OnlineState.LOGGING_IN;
        }
        
        public void RequestGameData() {
            
        }

        public void Connect() {
            this.State = OnlineState.CONNECTED;
            this.Socket.Connect();
        }
        public void Close() { this.Socket.Close(); OnDisconnect(); }
        public void CloseAsync() { this.Socket.CloseAsync(); OnDisconnect(); }

        public void SendBytes(byte[] bytes) => this.Socket.Send(bytes);
        public void SendString(string data) => this.Socket.Send(data);

        public bool IsConnected => this.Socket.IsAlive;
    }

    public enum OnlineState {
        OFFLINE,
        CONNECTED,
        LOGGING_IN,
        LOGGED_IN,
        IN_GAME,
    }
}
