using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine.Graphics.Sprites;

namespace KawashimaBattleRoyaleClient.Screens {
    public class UIComponentGameplayScreen : DrawableGameComponent {
        public UIComponentGameplayScreen() : base(pKawashimaGame.Game) {}
        
        protected SpriteManager SpriteManager = new();
        
        public override void Draw(GameTime gameTime) {
            this.SpriteManager.Draw();

            base.Draw(gameTime);
        }

        public override void Initialize() {
            pKawashimaGame.Socket.State = OnlineState.IN_GAME;
            pKawashimaGame.Socket.RequestGameData();
            
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
