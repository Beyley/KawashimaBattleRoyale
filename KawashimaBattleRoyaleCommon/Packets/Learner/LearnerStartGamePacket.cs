namespace KawashimaBattleRoyaleCommon.Packets.Learner {
	public class LearnerStartGamePacket : Packet {
		public LearnerStartGamePacket() {
			this.PacketType = PacketType.LEARNER_JOIN_GAME;
		}

		public override string GetData() {
			return "{}";
		}
		public override void ParseData(string data) {}
	}
}
