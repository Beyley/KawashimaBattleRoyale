namespace KawashimaBattleRoyaleCommon.Packets.Learner {
	public class LearnerEndGamePacket : Packet {
		public LearnerEndGamePacket() {
			this.PacketType = PacketType.LEARNER_LEAVE_GAME;
		}

		public override string GetData() {
			return "{}";
		}
		public override void ParseData(string data) {}
	}
}
