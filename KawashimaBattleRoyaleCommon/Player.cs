namespace KawashimaBattleRoyaleCommon {
	public class Player {
		public Player(string Username, int UserId, bool ingame) {
			this.Username = Username;
			this.UserId   = UserId;
			this.Ingame   = ingame;
		}
		public string Username { get; set; }
		public int    UserId   { get; set; }
		public bool   Ingame   { get; set; }
	}
}
