using System;
using KawashimaBattleRoyaleCommon.Packets.Learner;
using Newtonsoft.Json;

namespace KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima {
    public class DrKawashimaLearnerLoginPacket : Packet {
        public string Username { get; set; }
        public int    Userid   { get; set; }

        public DrKawashimaLearnerLoginPacket(string username = null, int userid = -1) {
            this.PacketType = PacketType.DRKAWASHIMA_LEARNER_LOGIN;

            this.Username = username;
            this.Userid = userid;
        }

        public override string GetData() {
            return JsonConvert.SerializeObject(this);
        }
        public override void ParseData(string data) {
            DrKawashimaLearnerLoginPacket parsedPacket = JsonConvert.DeserializeObject<DrKawashimaLearnerLoginPacket>(data);

            if (parsedPacket == null) throw new FormatException();
            
            this.Username = parsedPacket.Username;
            this.Userid   = parsedPacket.Userid;
        }
    }
}
