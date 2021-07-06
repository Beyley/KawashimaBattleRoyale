using System;
using Newtonsoft.Json;

namespace KawashimaBattleRoyaleCommon.Packets.Learner {
	public class LearnerLoginPacket : Packet {
		public LearnerLoginPacket(string username, string password) {
			this.PacketType = PacketType.LEARNER_LOGIN;

			this.username = username;
			this.password = password;
		}
		public string username { get; set; }
		public string password { get; set; }

		public override string GetData() {
			return JsonConvert.SerializeObject(this);
		}
		public override void ParseData(string data) {
			var parsedPacket = JsonConvert.DeserializeObject<LearnerLoginPacket>(data);

			if (parsedPacket == null) throw new FormatException();

			this.username = parsedPacket.username;
			this.password = parsedPacket.password;
		}
	}
}
