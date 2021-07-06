using System;
using Newtonsoft.Json;

namespace KawashimaBattleRoyaleCommon.Packets.Learner {
	public class LearnerSendQuestionPacket : Packet {
		public LearnerSendQuestionPacket(ProblemTypes type) {
			this.PacketType = PacketType.LEARNER_SEND_QUESTION;

			this.type = type;
		}
		public ProblemTypes type { get; set; }

		public override string GetData() {
			return JsonConvert.SerializeObject(this);
		}
		public override void ParseData(string data) {
			var parsedPacket = JsonConvert.DeserializeObject<LearnerSendQuestionPacket>(data);

			if (parsedPacket == null) throw new FormatException();

			this.type = parsedPacket.type;
		}
	}
}
