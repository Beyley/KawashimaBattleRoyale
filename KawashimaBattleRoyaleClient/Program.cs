namespace KawashimaBattleRoyaleClient {
	internal class Program {
		private static void Main(string[] args) {
			using (var game = new pKawashimaGame()) {
				game.Run();
			}
		}
	}
}
