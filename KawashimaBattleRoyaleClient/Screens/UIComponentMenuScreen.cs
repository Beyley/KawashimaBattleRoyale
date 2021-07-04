using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine;
using PeppyCodeEngineGL.Engine.Audio;
using PeppyCodeEngineGL.Engine.Graphics.Notifications;
using PeppyCodeEngineGL.Engine.Graphics.Renderers;
using PeppyCodeEngineGL.Engine.Graphics.Sprites;
using PeppyCodeEngineGL.Engine.Graphics.UserInterface;
using PeppyCodeEngineGL.Engine.Input;

namespace KawashimaBattleRoyaleClient.Screens {
    public class UIComponentMenuScreen : DrawableGameComponent {
        public UIComponentMenuScreen() : base(pKawashimaGame.Game) {}
        
        protected SpriteManager SpriteManager = new();

        private pTextBox usernameInputBox;
        public pText onlineUserCount;

        public override void Draw(GameTime gameTime) {
            this.SpriteManager.Draw();

            base.Draw(gameTime);
        }

        public override void Initialize() {
            float y = 10;

            pButton button = new pButton("Login/Connect!", new Vector2(10, y), new Vector2(150, 25),1, Color.Blue, delegate(object? sender, EventArgs args) {
                Debug.Assert(pKawashimaGame.OnlineManager != null, "pKawashimaGame.OnlineManager is null!");
                if (!pKawashimaGame.OnlineManager.IsConnected) {
                    pKawashimaGame.OnlineManager.Connect();
                    
                    //TODO implement passwords
                    pKawashimaGame.OnlineManager.Login(this.usernameInputBox.Box.Text, "");

                } else
                    NotificationManager.CreateNotification(NotificationManager.NotificationType.LeftBlob, Color.Red, "You are already online!", 5000);
            });
            y += 35;
            this.SpriteManager.Add(button.SpriteCollection);

            usernameInputBox = new pTextBox("username", 15, new Vector2(10, y), new Vector2(150, 25), 1);
            y += 35;
            this.SpriteManager.Add(usernameInputBox.SpriteCollection);
            
            button = new pButton("Start!", new Vector2(10, y), new Vector2(150, 25),1, Color.Blue, delegate(object? sender, EventArgs args) {
                if (pKawashimaGame.OnlineManager != null && pKawashimaGame.OnlineManager.LoggedIn)
                    pKawashimaGame.ChangeMode(new UIComponentGameplayScreen());
                else
                    NotificationManager.CreateNotification(NotificationManager.NotificationType.LeftBlob, Color.Red, "You need to be logged in!", 5000);
            });
            y += 40;
            this.SpriteManager.Add(button.SpriteCollection);

            onlineUserCount = new pText($"Online Players: {pKawashimaGame.OnlineManager.OnlineClients.Count + (pKawashimaGame.OnlineManager.LoggedIn ? 1 : 0)}", 20, new Vector2(10, y), 1, true, Color.White);
            this.SpriteManager.Add(onlineUserCount);

            Debug.Assert(pKawashimaGame.OnlineManager != null, "pKawashimaGame.OnlineManager is null!");
            pKawashimaGame.OnlineManager.onPlayersChanged += onPlayersChanged;
            
            pKawashimaGame.LoadComplete();

            base.Initialize();
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            // this.renderer.EmitterLocation = InputManager.CursorPosition;
        }

        private void onPlayersChanged(object? sender, object args) {
            onlineUserCount.Text = $"Online Players: {pKawashimaGame.OnlineManager.OnlineClients.Count + 1}";
        }
        
        protected override void Dispose(bool disposing) {
            this.SpriteManager.Dispose();
            Debug.Assert(pKawashimaGame.OnlineManager != null, "pKawashimaGame.OnlineManager is null!");
            pKawashimaGame.OnlineManager.onPlayersChanged -= onPlayersChanged;

            base.Dispose(disposing);
        }
    }
}
