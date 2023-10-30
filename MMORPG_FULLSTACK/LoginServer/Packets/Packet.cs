using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Packets
{
    public class Packet : PacketBase
    {

        public PacketType Type { get; private set; }
        public string Data { get; private set; }

        public Packet(PacketType type, string data)
        {
            Type = type;
            Data = data;
        }

        public override void Deserialize(byte[] data)
        {
            string strData = Encoding.UTF8.GetString(data).TrimEnd('\0'); // Enlevez les octets nuls à la fin
            string[] parts = strData.Split('|');
            if (parts.Length < 3) // Envisagez d'utiliser une meilleure vérification pour éviter des erreurs
                throw new ArgumentException("Data is not a valid packet.");

            Nonce = parts[0];
            HMAC = parts[1];
            Type = (PacketType)Enum.Parse(typeof(PacketType), parts[2]);
            Data = parts.Length > 3 ? parts[3] : "";
        }

        public override byte[] Serialize()
        {
            string strData = $"{Nonce}|{HMAC}|{Type}|{Data}";
            return Encoding.UTF8.GetBytes(strData);
        }

        // Vous pourriez également envisager d'ajouter des méthodes pour gérer le HMAC, le nonce, etc.
    }
}
