using KawashimaBattleRoyaleClient.Screens;
using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine;

namespace KawashimaBattleRoyaleClient {
    public class pKawashimaGame : pEngineGame {
        public static OnlineHandler Socket;
        
        public pKawashimaGame() : base(new UIComponentMenuScreen()) {
            this.IsMouseVisible = true;
        }

        protected override void Initialize() {
            Socket = new OnlineHandler("ws://127.0.0.1:1231/kawashima/");
            // Socket.Connect();

            base.Initialize();
        }
    }
}
