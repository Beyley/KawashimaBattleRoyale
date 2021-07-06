using System;
using Newtonsoft.Json;

namespace KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima {
	public class DrKawashimaLearnerLeavePacket : Packet {
		public DrKawashimaLearnerLeavePacket(int userid) {
			this.PacketType = PacketType.DRKAWASHIMA_LEARNER_LEAVE;

			this.userid = userid;
		}
		public int userid { get; set; }

		public override string GetData() {
			return JsonConvert.SerializeObject(this);
		}
		public override void ParseData(string data) {
			var parsedPacket = JsonConvert.DeserializeObject<DrKawashimaLearnerLeavePacket>(data);

			if (parsedPacket == null) throw new FormatException();

			this.userid = parsedPacket.userid;
		}
	}
}
