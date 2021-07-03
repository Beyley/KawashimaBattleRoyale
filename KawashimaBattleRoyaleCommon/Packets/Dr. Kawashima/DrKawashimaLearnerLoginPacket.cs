using System;
using KawashimaBattleRoyaleCommon.Packets.Learner;
using Newtonsoft.Json;

namespace KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima {
    public class DrKawashimaLearnerLoginPacket : Packet {
        public string username { get; set; }
        public int    userid   { get; set; }

        public DrKawashimaLearnerLoginPacket(string username) {
            this.PacketType = PacketType.DRKAWASHIMA_LEARNER_LOGIN;

            this.username = username;
        }

        public override string GetData() {
            return JsonConvert.SerializeObject(this);
        }
        public override void ParseData(string data) {
            DrKawashimaLearnerLoginPacket parsedPacket = JsonConvert.DeserializeObject<DrKawashimaLearnerLoginPacket>(data);

            if (parsedPacket == null) throw new FormatException();
            
            this.username = parsedPacket.username;
            this.userid   = parsedPacket.userid;
        }
    }
}
