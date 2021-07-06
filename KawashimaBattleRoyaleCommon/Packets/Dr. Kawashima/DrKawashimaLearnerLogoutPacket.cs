using System;
using Newtonsoft.Json;

namespace KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima {
	public class DrKawashimaLearnerLogoutPacket : Packet {
		public DrKawashimaLearnerLogoutPacket(int userid) {
			this.PacketType = PacketType.DRKAWASHIMA_LEARNER_LOGOUT;

			this.userid = userid;
		}
		public int userid { get; set; }

		public override string GetData() {
			return JsonConvert.SerializeObject(this);
		}
		public override void ParseData(string data) {
			var parsedPacket = JsonConvert.DeserializeObject<DrKawashimaLearnerLogoutPacket>(data);

			if (parsedPacket == null) throw new FormatException();

			this.userid = parsedPacket.userid;
		}
	}
}
