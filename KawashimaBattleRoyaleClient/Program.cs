using System;

namespace KawashimaBattleRoyaleClient {
    class Program {
        static void Main(string[] args) {
            using (var game = new pKawashimaGame())
                game.Run();
        }
    }
}
