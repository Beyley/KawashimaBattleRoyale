using System;
using Newtonsoft.Json;

namespace KawashimaBattleRoyaleCommon.Packets.Dr._Kawashima {
    public class DrKawashimaSendQuestionPacket : Packet {
        public ProblemTypes type { get; set; }

        public DrKawashimaSendQuestionPacket(ProblemTypes type) {
            this.PacketType = PacketType.DRKAWASHIMA_SEND_QUESTION;

            this.type = type;
        }

        public override string GetData() {
            return JsonConvert.SerializeObject(this);
        }
        public override void ParseData(string data) {
            DrKawashimaSendQuestionPacket parsedPacket = JsonConvert.DeserializeObject<DrKawashimaSendQuestionPacket>(data);

            if (parsedPacket == null) throw new FormatException();

            this.type = parsedPacket.type;
        }
    }
}
