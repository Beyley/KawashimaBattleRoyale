namespace KawashimaBattleRoyaleCommon.Packets.Learner {
	public class LearnerGetGameDataPacket : Packet {
		public LearnerGetGameDataPacket() {
			this.PacketType = PacketType.LEARNER_GET_GAME_DATA;
		}

		public override string GetData() {
			return "{}";
		}
		public override void ParseData(string data) {}
	}
}
