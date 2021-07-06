namespace KawashimaBattleRoyaleCommon {
	public abstract class Serializable {
		public abstract string Serialize();
		public abstract void   Deserialize(string data);
	}
}
