using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Packets
{
    public abstract class PacketBase
    {
        // Définir des propriétés communes à tous les paquets
        public string Nonce { get; set; } // Utilisé pour garantir l'unicité de chaque paquet
        public string HMAC { get; set; }  // HMAC pour vérifier l'intégrité du paquet

        public abstract byte[] Serialize();
        public abstract void Deserialize(byte[] data);

        // D'autres méthodes liées à la manipulation de base des paquets
    }
}
