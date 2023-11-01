using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Packets
{
    public class Notification : Packet
    {
        public string Message { get; private set; }

        public Notification(string message) : base(PacketType.Notification, message)
        {
            Message = message;
        }

        public override void Deserialize(byte[] data)
        {
            base.Deserialize(data);

            string[] parts = Data.Split(':');
            if (parts.Length != 1)
                throw new ArgumentException("Invalid Notification format.");

            Message = parts[0];
        }
    }

}
