using System;
using System.IO;
using System.Text;

namespace KawashimaBattleRoyaleCommon {
    public abstract class Packet : Serializable {
        public PacketType PacketType;
        
        /// <summary>
        /// Gets the data of the packet as JSON
        /// </summary>
        /// <returns>The JSON of the object</returns>
        public abstract string GetData();
        /// <summary>
        /// Parses the data into the object
        /// </summary>
        /// <param name="data">The JSON of the data to be parsed into an object</param>
        public abstract void ParseData(string data);

        /// <summary>
        /// Serializes an object into JSON
        /// </summary>
        /// <returns>The serialized object</returns>
        public override string Serialize() {
            StringBuilder builder = new();

            builder.Append($"{(uint) PacketType}\n");
            builder.Append(this.GetData());
            
            return builder.ToString();
        }

        /// <summary>
        /// Deserializes the object from JSON
        /// </summary>
        /// <param name="data">The data to deserialize</param>
        /// <exception cref="FormatException">Called when the data is of length 0</exception>
        public override void Deserialize(string data) {
            StringReader reader = new(data);
            if (data.Length == 0)
                throw new FormatException("The data was of length 0");
            
            PacketType = (PacketType) uint.Parse(reader.ReadLine());
            
            ParseData(reader.ReadLine());
        }
    }
} 
