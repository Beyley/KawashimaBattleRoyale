using System;
using Newtonsoft.Json;

namespace KawashimaBattleRoyaleCommon.Packets.Learner {
    public class LearnerLoginPacket : Packet {
        public string username { get; set; }
        public string password { get; set; }

        public LearnerLoginPacket(string username = null, string password = null) {
            this.PacketType = PacketType.LEARNER_LOGIN;

            this.username = username;
            this.password = password;
        }

        public override string GetData() {
            return JsonConvert.SerializeObject(this);
        }
        public override void ParseData(string data) {
            LearnerLoginPacket parsedPacket = JsonConvert.DeserializeObject<LearnerLoginPacket>(data);

            if (parsedPacket == null) throw new FormatException();
            
            this.username = parsedPacket.username;
            this.password = parsedPacket.password;
        }
    }
}
