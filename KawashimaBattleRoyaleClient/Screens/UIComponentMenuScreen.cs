using System;
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

        public override void Draw(GameTime gameTime) {
            this.SpriteManager.Draw();

            base.Draw(gameTime);
        }

        public override void Initialize() {
            float y = 10;

            pButton button = new pButton("Login!", new Vector2(10, y), new Vector2(150, 25),1, Color.Blue, delegate(object? sender, EventArgs args) {
                if (!pKawashimaGame.Socket.IsConnected) {
                    pKawashimaGame.Socket.Connect();
                    pKawashimaGame.Socket.username = this.usernameInputBox.Box.Text;
                    pKawashimaGame.Socket.SendString(this.usernameInputBox.Box.Text);

                }
            });
            y += 35;
            this.SpriteManager.Add(button.SpriteCollection);

            usernameInputBox = new pTextBox("username", 15, new Vector2(10, y), new Vector2(150, 25), 1);
            y += 35;
            this.SpriteManager.Add(usernameInputBox.SpriteCollection);
            
            button = new pButton("Start!", new Vector2(10, y), new Vector2(150, 25),1, Color.Blue, delegate(object? sender, EventArgs args) {
                if (pKawashimaGame.Socket.IsLoggedIn)
                    pKawashimaGame.ChangeMode(new UIComponentGameplayScreen());
                else
                    NotificationManager.CreateNotification(NotificationManager.NotificationType.LeftBlob, Color.Red, "You need to be logged in!", 5000);
            });
            y += 35;
            this.SpriteManager.Add(button.SpriteCollection);
            
            pKawashimaGame.LoadComplete();

            base.Initialize();
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            // this.renderer.EmitterLocation = InputManager.CursorPosition;
        }

        protected override void Dispose(bool disposing) {
            this.SpriteManager.Dispose();

            base.Dispose(disposing);
        }
    }
}
