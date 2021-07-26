using System.Diagnostics;
using Microsoft.Xna.Framework;
using PeppyCodeEngineGL.Engine;
using PeppyCodeEngineGL.Engine.Graphics;
using PeppyCodeEngineGL.Engine.Graphics.Notifications;
using PeppyCodeEngineGL.Engine.Graphics.Sprites;
using PeppyCodeEngineGL.Engine.Graphics.UserInterface;
using PeppyCodeEngineGL.Engine.State;

namespace KawashimaBattleRoyaleClient.Screens {
	public class UIComponentMenuScreen : Screen {
		public pText onlineUserCount;

		protected SpriteManager SpriteManager = new();

		private pTextBox usernameInputBox;
		public UIComponentMenuScreen() : base(pEngineGame.Game) {}

		public override void Draw(GameTime gameTime) {
			this.SpriteManager.Draw();

			base.Draw(gameTime);
		}

		public override void Initialize() {
			float y = 10;

			pButton button = new("Login/Connect!", new Vector2(10, y), new Vector2(150, 25), 1, Color.Blue, delegate {
				Debug.Assert(pKawashimaGame.OnlineManager != null, "pKawashimaGame.OnlineManager is null!");
				if (!pKawashimaGame.OnlineManager.IsConnected) {
					pKawashimaGame.OnlineManager.Connect();

					//TODO implement passwords
					pKawashimaGame.OnlineManager.Login(this.usernameInputBox.Box.Text, "");
				}
				else {
					NotificationManager.CreateNotification(NotificationManager.NotificationType.LeftBlob, Color.Red, "You are already online!", 5000);
				}
			});
			y += 45;
			this.SpriteManager.Add(button.SpriteCollection);

			this.usernameInputBox =  new pTextBox("username", 15, new Vector2(10, y), new Vector2(200, 30), 1);
			y                     += 35;
			this.SpriteManager.Add(this.usernameInputBox.SpriteCollection);

			button = new pButton("Start!", new Vector2(10, y), new Vector2(150, 25), 1, Color.Blue, delegate {
				if (pKawashimaGame.OnlineManager != null && pKawashimaGame.OnlineManager.LoggedIn) {
					if (pKawashimaGame.GameplayScreen == null) {
						pKawashimaGame.GameplayScreen = new UIComponentGameplayScreen();
					}
					else {
						pKawashimaGame.GameplayScreen.Dispose();
						pKawashimaGame.GameplayScreen = new UIComponentGameplayScreen();
					}

					ScreenManager.ChangeScreen(pKawashimaGame.GameplayScreen);
				}
				else {
					NotificationManager.CreateNotification(NotificationManager.NotificationType.LeftBlob, Color.Red, "You need to be logged in!", 5000);
				}
			});
			y += 40;
			this.SpriteManager.Add(button.SpriteCollection);

			this.onlineUserCount = new pText($"Online Players: {pKawashimaGame.OnlineManager.OnlineClients.Count + (pKawashimaGame.OnlineManager.LoggedIn ? 1 : 0)}", 20, new Vector2(10, y), 1, true, Color.White);
			this.SpriteManager.Add(this.onlineUserCount);

			Debug.Assert(pKawashimaGame.OnlineManager != null, "pKawashimaGame.OnlineManager is null!");
			pKawashimaGame.OnlineManager.onPlayersChanged += this.onPlayersChanged;

			pEngineGame.LoadComplete();

			pEngineGame.Game.IsMouseVisible = true;

			base.Initialize();
		}
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			// this.renderer.EmitterLocation = InputManager.CursorPosition;
		}

		private void onPlayersChanged(object? sender, object args) {
			this.onlineUserCount.Text = $"Online Players: {pKawashimaGame.OnlineManager.OnlineClients.Count + 1}";
		}

		protected override void Dispose(bool disposing) {
			this.SpriteManager.Dispose();
			Debug.Assert(pKawashimaGame.OnlineManager != null, "pKawashimaGame.OnlineManager is null!");
			pKawashimaGame.OnlineManager.onPlayersChanged -= this.onPlayersChanged;

			base.Dispose(disposing);
		}
	}
}
