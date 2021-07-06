using System;
using Newtonsoft.Json;

namespace KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima {
	public class DrKawashimaLearnerJoinPacket : Packet {
		public DrKawashimaLearnerJoinPacket(int userid) {
			this.PacketType = PacketType.DRKAWASHIMA_LEARNER_JOIN;

			this.userid = userid;
		}
		public int userid { get; set; }

		public override string GetData() {
			return JsonConvert.SerializeObject(this);
		}
		public override void ParseData(string data) {
			var parsedPacket = JsonConvert.DeserializeObject<DrKawashimaLearnerJoinPacket>(data);

			if (parsedPacket == null) throw new FormatException();

			this.userid = parsedPacket.userid;
		}
	}
}
