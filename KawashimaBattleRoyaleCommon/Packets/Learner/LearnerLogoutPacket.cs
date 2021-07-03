namespace KawashimaBattleRoyaleCommon.Packets.Learner {
    public class LearnerLogoutPacket : Packet{
        public LearnerLogoutPacket() {
            this.PacketType = PacketType.LEARNER_LOGOUT;
        }

        public override string GetData() {
            return "{}";
        }
        public override void ParseData(string data) {

        }
    }
}
