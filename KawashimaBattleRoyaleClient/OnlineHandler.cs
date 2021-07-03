using System;
using KawashimaBattleRoyaleClient.Screens;
using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine.Graphics.Notifications;
using WebSocketSharp;

namespace KawashimaBattleRoyaleClient {
    public class OnlineHandler {
        public WebSocket Socket;

        public bool IsLoggedIn;
        public string username;
        public bool isIngame;

        public OnlineHandler(string location) {
            this.Socket = new WebSocket(location);

            this.Socket.OnMessage += OnMessage;
        }

        private void OnMessage(object? sender, MessageEventArgs args) {
            if (args.Data == "logged in") {
                this.IsLoggedIn = true;
                NotificationManager.CreateNotification(NotificationManager.NotificationType.LeftBlob, Color.Blue, $"Logged In as {username}!", 1500);
                
                return;
            }
        }

        public void RequestGameData() {
            this.SendString("get gamedata");
        }
        
        public void Connect() => this.Socket.Connect();
        public void Close() => this.Socket.Close();
        public void CloseAsync() => this.Socket.CloseAsync();

        public void SendBytes(byte[] bytes) => this.Socket.Send(bytes);
        public void SendString(string data) => this.Socket.Send(data);

        public bool IsConnected => this.Socket.IsAlive;
    }
}
