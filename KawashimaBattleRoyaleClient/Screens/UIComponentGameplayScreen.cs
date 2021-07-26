using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KawashimaBattleRoyaleCommon;
using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine;
using PeppyCodeEngineGL.Engine.Graphics;
using PeppyCodeEngineGL.Engine.Graphics.Sprites;
using PeppyCodeEngineGL.Engine.Graphics.UserInterface;

namespace KawashimaBattleRoyaleClient.Screens {
	public class UIComponentGameplayScreen : Screen {
		private Tuple<string, double, bool, ProblemTypes> currentAnswer = new("", double.NaN, true, 0);

		private pText currentProblem;

		private pText flashText;

		private          pTextBox                                   inputBox;
		private          pText                                      levelIndicator;
		private readonly Queue<Tuple<string, double, ProblemTypes>> nextAnswers = new();
		private          pText                                      nextProblem;

		private int   problemsDone;
		private pText queueSize;

		private Random r;

		protected SpriteManager SpriteManager = new();

		public  GameState State = new(1);
		private Task      task;

		private Timer timer;
		public UIComponentGameplayScreen() : base(pEngineGame.Game) {}

		public override void Draw(GameTime gameTime) {
			this.SpriteManager.Draw();

			base.Draw(gameTime);
		}

		public override void Initialize() {
			this.r = new();

			//Tell the server that we are joining the game           
			pKawashimaGame.OnlineManager.JoinGame();

			//Initialize the sprites
			this.inputBox       = new pTextBox("0", 25, new Vector2(10, 10), new Vector2(200, 40), 1);
			this.queueSize      = new pText("Queue: 0", 30, new Vector2(225, 5), 1, true, Color.LightGray);
			this.nextProblem    = new pText("?+?=?", 30, new Vector2(10, 50), 1, true, Color.LightGray);
			this.currentProblem = new pText("?+?=?", 30, new Vector2(10, 100), 1, true, Color.White);
			this.levelIndicator = new pText("Level: 0", 25, new Vector2(10, 300), 1, true, Color.White);

			this.inputBox.OnCommit += (sender, args) => {
				if (this.currentAnswer.Item1 == "" || this.inputBox.Box.Text == "") return;

				try {
					if (Math.Abs(double.Parse(this.inputBox.Box.Text) - this.currentAnswer.Item2) < 1.5)
						this.OnRightAnswer();
					else
						this.OnWrongAnswer();
				}
				catch {
					this.OnWrongAnswer();
				}

				this.problemsDone++;
				if (this.problemsDone % 4 == 0) {
					this.State.Level++;

					this.levelIndicator.Text = $"Level: {this.State.Level}";
				}

				if (this.problemsDone % 2 == 0)
					pKawashimaGame.OnlineManager.SendQuestion(this.currentAnswer.Item4);


				this.inputBox.Box.Text = "";
				this.inputBox.FocusGot();
				this.currentAnswer = new(this.currentAnswer.Item1, this.currentAnswer.Item2, true, this.currentAnswer.Item4);

				this.nextProblem.Text = "?+?=?";

				// GenerateNewProblem();
			};

			this.flashText = new pText("", 50, new Vector2(200, 200), 0.5f, true, Color.White) {
				IsVisible = false
			};

			//Add the sprites to the sprite manager
			this.SpriteManager.Add(this.inputBox.SpriteCollection);
			this.SpriteManager.Add(this.currentProblem);
			this.SpriteManager.Add(this.nextProblem);
			this.SpriteManager.Add(this.queueSize);
			this.SpriteManager.Add(this.levelIndicator);
			this.SpriteManager.Add(this.flashText);

			this.GenerateNewProblem();
			// GenerateNewProblem();
			// GenerateNewProblem();
			// GenerateNewProblem();

			this.task = Task.Run(() => {
				while (true) // Console.WriteLine("tock");
					try {
						Thread.Sleep(100);
						if (this.currentAnswer.Item3) {
							try {
								// Console.WriteLine("test");
								Tuple<string, double, ProblemTypes> nextAnswer = this.nextAnswers.Dequeue();

								this.currentAnswer = new Tuple<string, double, bool, ProblemTypes>(nextAnswer.Item1, nextAnswer.Item2, false, nextAnswer.Item3);

								this.currentProblem.Text = this.currentAnswer.Item1;
							}
							catch {
								// ignored
							}
						}
						else {
							Tuple<string, double, ProblemTypes> nextAnswer = this.nextAnswers.Peek();

							this.nextProblem.Text = nextAnswer.Item1;
						}

						this.queueSize.Text = $"Queue: {this.nextAnswers.Count}";

						if (this.nextAnswers.Count > 10)
							pEngineGame.ChangeMode(new UIComponentMenuScreen());

						if (this.nextAnswers.Count == 0 && this.currentAnswer.Item3)
							this.currentProblem.Text = "?+?=?";
					}
					catch {}
			});

			this.timer = new(this.QuestionTimerTick, new AutoResetEvent(false), 0, 5000);

			pEngineGame.LoadComplete();

			pEngineGame.Game.IsMouseVisible = true;

			base.Initialize();
		}

		public void QuestionTimerTick(object? stateInfo) {
			// Console.WriteLine("tick");
			this.GenerateNewProblem();

			((AutoResetEvent) stateInfo!).Set();
		}

		public void ResetTimer() {
			this.timer.Dispose();

			var period = 5000;
			if (this.State.Level > 7)
				period = 3000;

			this.timer = new(this.QuestionTimerTick, new AutoResetEvent(false), 0, period);
		}

		public void GenerateNewProblem(ProblemTypes type = (ProblemTypes) 1000) {
			// ProblemTypes type;

			if ((int) type == 1000)
				if (this.State.Level <= 3)
					type = (ProblemTypes) this.r.Next((int) ProblemTypes.ADDITION, (int) ProblemTypes.SUBTRACT + 1);
				else if (this.State.Level <= 5)
					type = (ProblemTypes) this.r.Next((int) ProblemTypes.ADDITION, (int) ProblemTypes.MULTIPLY + 1);
				else if (this.State.Level <= 10)
					type = (ProblemTypes) this.r.Next((int) ProblemTypes.ADDITION, (int) ProblemTypes.DIVIDE + 1);
				else if (this.State.Level <= 15)
					type = (ProblemTypes) this.r.Next((int) ProblemTypes.ADDITION, (int) ProblemTypes.MODULO + 1);
				else if (this.State.Level <= 20)
					type = (ProblemTypes) this.r.Next((int) ProblemTypes.ADDITION, (int) ProblemTypes.POWER + 1);
				else
					type = (ProblemTypes) this.r.Next((int) ProblemTypes.MULTIPLY, (int) ProblemTypes.FACTORIAL + 1);

			switch (type) {
				case ProblemTypes.ADDITION: {
					var max = this.State.Level <= 10 ? 20 : 50;

					var terms  = this.State.Level <= 15 ? 2 : 2 + this.r.Next(this.State.Level - 13);
					var result = 0;

					string finalString = "";

					for (var i = 0; i < terms; i++) {
						var term = this.r.Next(max);

						if (i == 0)
							result = term;
						else
							result += term;

						finalString += $"{term} + ";
					}

					finalString =  finalString.Remove(finalString.Length - 3);
					finalString += " = ?";

					this.nextAnswers.Enqueue(new(finalString, result, type));
					// Console.WriteLine("adding");

					break;
				}
				case ProblemTypes.SUBTRACT: {
					var max = this.State.Level <= 10 ? 15 : 40;

					var terms  = this.State.Level <= 15 ? 2 : 2 + this.r.Next(this.State.Level - 13);
					var result = 0;

					string finalString = "";

					for (var i = 0; i < terms; i++) {
						var term = this.r.Next(max);

						if (i == 0)
							result = term;
						else
							result -= term;

						finalString += $"{term} - ";
					}

					finalString =  finalString.Remove(finalString.Length - 3);
					finalString += " = ?";

					this.nextAnswers.Enqueue(new(finalString, result, type));
					// Console.WriteLine("adding");

					break;
				}
				case ProblemTypes.MULTIPLY: {
					var max = this.State.Level <= 10 ? 7 : 12;

					var terms  = this.State.Level <= 20 ? 2 : 2 + this.r.Next(this.State.Level - 18);
					var result = 0;

					string finalString = "";

					for (var i = 0; i < terms; i++) {
						var term = this.r.Next(max);

						if (i == 0)
							result = term;
						else
							result *= term;

						finalString += $"{term} * ";
					}

					finalString =  finalString.Remove(finalString.Length - 3);
					finalString += " = ?";

					this.nextAnswers.Enqueue(new(finalString, result, type));
					// Console.WriteLine("adding");

					break;
				}
				case ProblemTypes.DIVIDE: {
					var max = this.State.Level <= 15 ? 5 : 10;

					var terms  = this.State.Level <= 20 ? 2 : 2 + this.r.Next(this.State.Level - 18);
					var result = 0;

					string finalString = "";

					for (var i = 0; i < terms; i++) {
						var term = this.r.Next(max);

						if (i == 0)
							result = term;
						else
							result /= term;

						finalString += $"{term} ÷ ";
					}

					finalString =  finalString.Remove(finalString.Length - 3);
					finalString += " = ?";

					this.nextAnswers.Enqueue(new(finalString, result, type));
					// Console.WriteLine("adding");

					break;
				}
				case ProblemTypes.MODULO: {
					var max = this.State.Level <= 15 ? 5 : 10;

					var terms  = 2;
					var result = 0;

					string finalString = "";

					for (var i = 0; i < terms; i++) {
						var term = this.r.Next(max);

						if (i == 0)
							result = term;
						else
							result %= term;

						finalString += $"{term} % ";
					}

					finalString =  finalString.Remove(finalString.Length - 3);
					finalString += " = ?";

					this.nextAnswers.Enqueue(new(finalString, result, type));
					// Console.WriteLine("adding");

					break;
				}
				case ProblemTypes.SQUAREROOT: {
					var max = this.State.Level <= 15 ? 10 : 15;

					string finalString = "";

					var result = Math.Sqrt(this.r.Next(max));

					finalString = $"√{result}";

					this.nextAnswers.Enqueue(new(finalString, result, type));
					// Console.WriteLine("adding");

					break;
				}
				case ProblemTypes.POWER: {
					var max = this.State.Level <= 20 ? 5 : 10;

					var terms  = 2;
					var result = 0;

					string finalString = "";

					for (var i = 0; i < terms; i++) {
						var term = this.r.Next(max);

						if (i == 0)
							result = term;
						else
							result %= term;

						finalString += $"{term} ^ ";
					}

					finalString =  finalString.Remove(finalString.Length - 3);
					finalString += " = ?";

					this.nextAnswers.Enqueue(new(finalString, result, type));
					// Console.WriteLine("adding");

					break;
				}
				case ProblemTypes.FACTORIAL: {
					var max = this.State.Level <= 30 ? 6 : 12;

					double result = Enumerable.Range(1, this.r.Next(max)).Aggregate(1, (p, item) => p * item);

					string finalString = $"{result}!";

					this.nextAnswers.Enqueue(new(finalString, result, type));
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
			this.FlashText("Correct!", Color.Green, 1000);
		}

		public void OnWrongAnswer() {
			this.FlashText("Wrong!", Color.Red, 1000);
		}

		public void FlashText(string text, Color color, int duration) {
			this.flashText.Text       = text;
			this.flashText.TextColour = color;

			this.flashText.IsVisible = true;

			this.flashText.Transformations.Add(new(TransformationType.Fade, 1, 0, pEngineGame.GetTime(ClockTypes.Game), pEngineGame.GetTime(ClockTypes.Game) + duration));
		}

		// public override void Update(GameTime gameTime) {
		//     base.Update(gameTime);
		//
		//     // this.renderer.EmitterLocation = InputManager.CursorPosition;
		// }

		protected override void Dispose(bool disposing) {
			this.SpriteManager.Dispose();
			this.timer.Dispose();
			this.task.Dispose();

			pKawashimaGame.OnlineManager.LeaveGame();

			base.Dispose(disposing);
		}
	}
}
