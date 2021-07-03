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

        public override void Draw(GameTime gameTime) {
            this.SpriteManager.Draw();

            base.Draw(gameTime);
        }

        public override void Initialize() {
            pButton button;

            button = new pButton("Send test data!", new Vector2(10, 10), new Vector2(150, 25),1, Color.Blue, delegate(object? sender, EventArgs args) {
                if (pKawashimaGame.Socket.IsConnected) {
                    pKawashimaGame.Socket.SendString("Hello World!");
                }
            });
            
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
