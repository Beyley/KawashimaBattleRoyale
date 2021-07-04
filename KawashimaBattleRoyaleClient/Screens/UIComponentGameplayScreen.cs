using System;
using KawashimaBattleRoyaleCommon;
using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine.Graphics.Notifications;
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

        public GameState State = new(1);

        public override void Draw(GameTime gameTime) {
            this.SpriteManager.Draw();

            base.Draw(gameTime);
        }

        public override void Initialize() {
            r = new();
            
            //Tell the server that we are joining the game           
            pKawashimaGame.OnlineManager.JoinGame();

            //Initialize the sprites
            inputBox = new pTextBox("0", 15, new Vector2(10,10), new Vector2(100,25), 1);
            currentProblem = new pText("?x?=?", 15, new Vector2(10, 50), 1, true, Color.White);

            inputBox.OnCommit += (sender, args) => {
                if (double.Parse(inputBox.Box.Text) == currentAnswer) 
                    OnRightAnswer();
                else
                    OnWrongAnswer();

                inputBox.Box.Text = "";
                inputBox.FocusGot();
                
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
            ProblemTypes type;
            
            if (State.Level <= 3) {
                type = (ProblemTypes)r.Next((int)ProblemTypes.ADDITION, (int)ProblemTypes.SUBTRACT+1);
            } else if(State.Level <= 5) {
                type = (ProblemTypes)r.Next((int)ProblemTypes.ADDITION, (int)ProblemTypes.MULTIPLY+1);
            } else if (State.Level <= 10) {
                type = (ProblemTypes)r.Next((int)ProblemTypes.ADDITION, (int)ProblemTypes.DIVIDE+1);
            } else if (State.Level <= 15) {
                type = (ProblemTypes)r.Next((int)ProblemTypes.ADDITION, (int)ProblemTypes.MODULO+1);
            } else if (State.Level <= 20) {
                type = (ProblemTypes)r.Next((int)ProblemTypes.ADDITION, (int)ProblemTypes.POWER+1);
            } else {
                type = (ProblemTypes)r.Next((int)ProblemTypes.ADDITION, (int)ProblemTypes.FACTORIAL+1);
            }

            switch (type) {
                case ProblemTypes.ADDITION: {
                    int max;

                    if (State.Level <= 10)
                        max = 20;
                    else
                        max = 50;

                    int terms = State.Level <= 15 ? 2 : 2 + r.Next(State.Level - 15);
                    int result = 0;

                    string finalString = "";
                    
                    for (int i = 0; i < terms; i++) {
                        int term = r.Next(max);
                        
                        result += term;

                        finalString += $"{term} + ";
                    }

                    finalString = finalString.Remove(finalString.Length - 3);
                    finalString += " = ?";

                    this.currentProblem.Text = finalString;

                    if (result == currentAnswer) {
                        GenerateNewProblem();
                        
                        return;
                    }
                    currentAnswer = result;
                    
                    break;
                }
                case ProblemTypes.SUBTRACT: {
                    int max;

                    if (State.Level <= 10)
                        max = 15;
                    else
                        max = 40;

                    int terms = State.Level <= 15 ? 2 : 2 + r.Next(State.Level - 15);
                    int result = 0;

                    string finalString = "";
                    
                    for (int i = 0; i < terms; i++) {
                        int term = r.Next(max);

                        if (i == 0)
                            result = term;
                        else
                            result -= term;

                        finalString += $"{term} - ";
                    }

                    finalString = finalString.Remove(finalString.Length - 3);
                    finalString += " = ?";

                    this.currentProblem.Text = finalString;

                    if (result == currentAnswer) {
                        GenerateNewProblem();
                        
                        return;
                    }
                    currentAnswer = result;
                    
                    break;
                }
            }
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

    public enum ProblemTypes : int {
        ADDITION   = 1,
        SUBTRACT   = 2,
        MULTIPLY   = 3,
        DIVIDE     = 4,
        MODULO     = 5,
        SQUAREROOT = 6,
        POWER      = 7,
        FACTORIAL  = 8,
    }
}
