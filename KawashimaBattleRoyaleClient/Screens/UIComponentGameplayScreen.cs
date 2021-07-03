using System;
using KawashimaBattleRoyaleCommon;
using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine.Graphics.Sprites;
using PeppyCodeEngineGL.Engine.Graphics.UserInterface;

namespace KawashimaBattleRoyaleClient.Screens {
    public class UIComponentGameplayScreen : DrawableGameComponent {
        public UIComponentGameplayScreen() : base(pKawashimaGame.Game) {}
        
        protected SpriteManager SpriteManager = new();

        private pTextBox inputBox;
        private pText currentProblem;

        private double currentAnswer;

        private Random r;

        public GameState state;

        public override void Draw(GameTime gameTime) {
            this.SpriteManager.Draw();

            base.Draw(gameTime);
        }

        public override void Initialize() {
            r = new();
            
            //Tell the server that we are joining the game           
            pKawashimaGame.OnlineManager.JoinGame();

            //Initialize the sprites
            inputBox = new pTextBox("test", 15, new Vector2(10,10), new Vector2(100,25), 1);
            currentProblem = new pText("?x?=?", 15, new Vector2(10, 50), 1, true, Color.White);

            inputBox.OnCommit += (sender, args) => {
                if (double.Parse(inputBox.Box.Text) == currentAnswer) 
                    OnRightAnswer();
                else
                    OnWrongAnswer();

                GenerateNewProblem();
            };
            
            //Add the sprites to the sprite manager
            SpriteManager.Add(inputBox.SpriteCollection);
            SpriteManager.Add(currentProblem);
            
            GenerateNewProblem();
            
            pKawashimaGame.LoadComplete();

            base.Initialize();
        }

        public void GenerateNewProblem() {

        }

        public void OnRightAnswer() {
            
        }

        public void OnWrongAnswer() {
            
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

    public enum ProblemTypes {
        ADDITION,
        SUBTRACT,
        MULTIPLY,
        DIVIDE,
        MODULO,
        SQUAREROOT,
    }
}
