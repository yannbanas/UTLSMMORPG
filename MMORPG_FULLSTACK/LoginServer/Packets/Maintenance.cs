using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Packets
{
    public class Maintenance : Packet
    {
        public bool IsMaintenance { get; private set; }

        public string Message { get; private set; }

        public Maintenance(bool isMaintenance, string message = "")
            : base(PacketType.Maintenance, isMaintenance ? "TRUE" : "FALSE")
        {
            IsMaintenance = isMaintenance;
            Message = message;
        }

        public override void Deserialize(byte[] data)
        {
            base.Deserialize(data);

            string[] parts = Data.Split(':');
            if (parts.Length < 2)
                throw new ArgumentException("Invalid Maintenance format.");

            IsMaintenance = parts[0].Equals("TRUE");
            Message = parts[1];
        }

    }
}
