using System;
using Newtonsoft.Json;

namespace KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima {
    public class DrKawashimaLearnerLeavePacket : Packet {
        public int    userid   { get; set; }

        public DrKawashimaLearnerLeavePacket(int userid) {
            this.PacketType = PacketType.DRKAWASHIMA_LEARNER_LEAVE;

            this.userid = userid;
        }

        public override string GetData() {
            return JsonConvert.SerializeObject(this);
        }
        public override void ParseData(string data) {
            DrKawashimaLearnerLeavePacket parsedPacket = JsonConvert.DeserializeObject<DrKawashimaLearnerLeavePacket>(data);

            if (parsedPacket == null) throw new FormatException();
            
            this.userid   = parsedPacket.userid;
        }
    }
}
