using System;
using Newtonsoft.Json;

namespace KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima {
	public class DrKawashimaSendQuestionPacket : Packet {
		public DrKawashimaSendQuestionPacket(ProblemTypes type) {
			this.PacketType = PacketType.DRKAWASHIMA_SEND_QUESTION;

			this.type = type;
		}
		public ProblemTypes type { get; set; }

		public override string GetData() {
			return JsonConvert.SerializeObject(this);
		}
		public override void ParseData(string data) {
			var parsedPacket = JsonConvert.DeserializeObject<DrKawashimaSendQuestionPacket>(data);

			if (parsedPacket == null) throw new FormatException();

			this.type = parsedPacket.type;
		}
	}
}
