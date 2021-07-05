using KawashimaBattleRoyaleClient.Screens;
using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine;

namespace KawashimaBattleRoyaleClient {
    public class pKawashimaGame : pEngineGame {
        public static OnlineHandler? OnlineManager;

        public static UIComponentGameplayScreen GameplayScreen;
        
        public pKawashimaGame() : base(new UIComponentMenuScreen()) {
            this.IsMouseVisible = true;
        }

        protected override void Initialize() {
            OnlineManager = new OnlineHandler("ws://127.0.0.1:1231/kawashima/");
            // Socket.Connect();

            base.Initialize();
        }
    }
}
