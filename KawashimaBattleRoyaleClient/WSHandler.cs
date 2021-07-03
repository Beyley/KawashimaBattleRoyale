using System;
using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine.Graphics.Notifications;
using WebSocketSharp;

namespace KawashimaBattleRoyaleClient {
    public class WSHandler {
        public WebSocket Socket;
        
        public WSHandler(string location) {
            this.Socket = new WebSocket(location);

            this.Socket.OnMessage += OnMessage;
        }

        private void OnMessage(object? sender, MessageEventArgs args) {
            // NotificationManager.CreateNotification(NotificationManager.NotificationType.LeftBlob, Color.Red, args.Data, 5000);
            
            
        }
        
        public void Connect() => this.Socket.Connect();
        public void Close() => this.Socket.Close();
        public void CloseAsync() => this.Socket.CloseAsync();

        public void SendBytes(byte[] bytes) => this.Socket.Send(bytes);
        public void SendString(string data) => this.Socket.Send(data);

        public bool IsConnected => this.Socket.IsAlive;
    }
}
