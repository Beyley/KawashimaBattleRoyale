using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using KawashimaBattleRoyaleCommon;
using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine.Graphics.Notifications;
using PeppyCodeEngineGL.Engine.Graphics.Sprites;
using PeppyCodeEngineGL.Engine.Graphics.UserInterface;
using System.Threading;

namespace KawashimaBattleRoyaleClient.Screens {
    public class UIComponentGameplayScreen : DrawableGameComponent {
        public UIComponentGameplayScreen() : base(pKawashimaGame.Game) {}
        
        protected SpriteManager SpriteManager = new();

        private pTextBox inputBox;
        
        private pText currentProblem;
        private pText nextProblem;
        private pText queueSize;
        private pText levelIndicator;
        
        private pText flashText;

        private Timer timer;
        private Task  task;

        private Tuple<string, double, bool> currentAnswer = new("", double.NaN, true);
        private Queue<Tuple<string, double>> nextAnswers = new();

        private Random r;

        private int problemsDone = 0;

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
            inputBox = new pTextBox("0", 25, new Vector2(10,10), new Vector2(200,40), 1);
            queueSize = new pText("Queue: 0", 30, new Vector2(225, 5), 1, true, Color.LightGray);
            nextProblem = new pText("?+?=?", 30, new Vector2(10, 50), 1, true, Color.LightGray);
            currentProblem = new pText("?+?=?", 30, new Vector2(10, 100), 1, true, Color.White);
            levelIndicator = new pText("Level: 0", 25, new Vector2(10, 300), 1, true, Color.White);

            inputBox.OnCommit += (sender, args) => {
                if (currentAnswer.Item1 == "") return;

                try {
                    if (Math.Abs(double.Parse(inputBox.Box.Text) - currentAnswer.Item2) < 1.5)
                        OnRightAnswer();
                    else
                        OnWrongAnswer();
                } catch {
                    OnWrongAnswer();
                }

                problemsDone++;
                if (problemsDone % 4 == 0) {
                    this.State.Level++;

                    this.levelIndicator.Text = $"Level: {this.State.Level}";
                }
                

                inputBox.Box.Text = "";
                inputBox.FocusGot();
                this.currentAnswer = new(this.currentAnswer.Item1, this.currentAnswer.Item2, true);

                this.nextProblem.Text = "?+?=?";
                
                // GenerateNewProblem();
            };

            flashText = new pText("", 50, new Vector2(200, 200), 0.5f, true, Color.White) {
                IsVisible = false
            };
            
            //Add the sprites to the sprite manager
            SpriteManager.Add(inputBox.SpriteCollection);
            SpriteManager.Add(currentProblem);
            SpriteManager.Add(nextProblem);
            SpriteManager.Add(queueSize);
            SpriteManager.Add(levelIndicator);
            SpriteManager.Add(flashText);

            GenerateNewProblem();
            // GenerateNewProblem();
            // GenerateNewProblem();
            // GenerateNewProblem();
            
            task = Task.Run(() => {
                while (true) {
                    // Console.WriteLine("tock");
                    try {
                        Thread.Sleep(100);
                        if (this.currentAnswer.Item3) {
                            try {
                                // Console.WriteLine("test");
                                Tuple<string, double> nextAnswer = this.nextAnswers.Dequeue();

                                this.currentAnswer = new Tuple<string, double, bool>(nextAnswer.Item1, nextAnswer.Item2, false);

                                this.currentProblem.Text = currentAnswer.Item1;
                            }
                            catch {
                                // ignored
                            }
                        }
                        else {
                            Tuple<string, double> nextAnswer = this.nextAnswers.Peek();

                            this.nextProblem.Text = nextAnswer.Item1;
                        }

                        this.queueSize.Text = $"Queue: {nextAnswers.Count}";

                        if (this.nextAnswers.Count > 10) {
                            pKawashimaGame.ChangeMode(new UIComponentMenuScreen());
                        }

                        if (this.nextAnswers.Count == 0 && this.currentAnswer.Item3) {
                            this.currentProblem.Text = "?+?=?";
                        }
                    } catch {}
                }
            });

            this.timer = new(this.QuestionTimerTick, new AutoResetEvent(false), 0, 5000);

            pKawashimaGame.LoadComplete();

            base.Initialize();
        }   
        
        public void QuestionTimerTick(object? stateInfo) {
            // Console.WriteLine("tick");
            GenerateNewProblem();
            
            ((AutoResetEvent) stateInfo!)?.Set();
        }

        public void ResetTimer() {
            this.timer.Dispose();
            
            this.timer = new(this.QuestionTimerTick, new AutoResetEvent(false), 0, 10000);
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

                    int terms = State.Level <= 15 ? 2 : 2 + r.Next(State.Level - 13);
                    int result = 0;

                    string finalString = "";
                    
                    for (int i = 0; i < terms; i++) {
                        int term = r.Next(max);
                        
                        if (i == 0)
                            result = term;
                        else
                            result += term;

                        finalString += $"{term} + ";
                    }

                    finalString = finalString.Remove(finalString.Length - 3);
                    finalString += " = ?";

                    nextAnswers.Enqueue(new (finalString, result));
                    // Console.WriteLine("adding");
                    
                    break;
                }
                case ProblemTypes.SUBTRACT: {
                    int max;

                    if (State.Level <= 10)
                        max = 15;
                    else
                        max = 40;

                    int terms = State.Level <= 15 ? 2 : 2 + r.Next(State.Level - 13);
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

                    nextAnswers.Enqueue(new (finalString, result));
                    // Console.WriteLine("adding");

                    break;
                }
                case ProblemTypes.MULTIPLY: {
                    int max;

                    if (State.Level <= 10)
                        max = 7;
                    else
                        max = 12;

                    int terms = State.Level <= 20 ? 2 : 2 + r.Next(State.Level - 18);
                    int result = 0;

                    string finalString = "";
                    
                    for (int i = 0; i < terms; i++) {
                        int term = r.Next(max);
                        
                        if (i == 0)
                            result = term;
                        else
                            result *= term;

                        finalString += $"{term} * ";
                    }

                    finalString = finalString.Remove(finalString.Length - 3);
                    finalString += " = ?";

                    nextAnswers.Enqueue(new (finalString, result));
                    // Console.WriteLine("adding");
                    
                    break;
                }
                case ProblemTypes.DIVIDE: {
                    int max;

                    if (State.Level <= 10)
                        max = 5;
                    else
                        max = 10;

                    int terms = State.Level <= 20 ? 2 : 2 + r.Next(State.Level - 18);
                    int result = 0;

                    string finalString = "";
                    
                    for (int i = 0; i < terms; i++) {
                        int term = r.Next(max);
                        
                        if (i == 0)
                            result = term;
                        else
                            result /= term;

                        finalString += $"{term} ÷ ";
                    }

                    finalString = finalString.Remove(finalString.Length - 3);
                    finalString += " = ?";

                    nextAnswers.Enqueue(new (finalString, result));
                    // Console.WriteLine("adding");
                    
                    break;
                }
                default: {
                    this.currentProblem.Text = "UNKNOWN PROBLEM TYPE";
                    
                    break;
                }
            }
        }

        public void OnRightAnswer() {
            FlashText("Correct!", Color.Green, 1000);
        }

        public void OnWrongAnswer() {
            FlashText("Wrong!", Color.Red, 1000);
        }

        public void FlashText(string text, Color color, int duration) {
            this.flashText.Text = text;
            this.flashText.TextColour = color;

            this.flashText.IsVisible = true;
                
            this.flashText.Transformations.Add(new(TransformationType.Fade, 1, 0, pKawashimaGame.GetTime(ClockTypes.Game), pKawashimaGame.GetTime(ClockTypes.Game) + duration));
        }
        
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            // this.renderer.EmitterLocation = InputManager.CursorPosition;
        }

        protected override void Dispose(bool disposing) {
            this.SpriteManager.Dispose();
            this.timer.Dispose();
            this.task.Dispose();

            pKawashimaGame.OnlineManager.LeaveGame();
            
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
