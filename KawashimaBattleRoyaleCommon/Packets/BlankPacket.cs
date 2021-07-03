namespace KawashimaBattleRoyaleCommon.Packets {
    public class BlankPacket : Packet {

        public override string GetData() {
            return "{}";
        }
        public override void ParseData(string data) {
            
        }
    }
}
