using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Packets
{
    public class LoginResponse : Packet
    {
        public bool IsSuccess { get; private set; }
        public string Token { get; private set; }

        // Si la connexion est réussie, un token est fourni. Sinon, le token est null.
        public LoginResponse(bool isSuccess, string token = null) : base(PacketType.LoginResponse, isSuccess ? $"SUCCESS:{token}" : "FAIL")
        {
            IsSuccess = isSuccess;
            Token = token;
        }

        public override void Deserialize(byte[] data)
        {
            base.Deserialize(data); // Appel de la méthode Deserialize de Packet

            string[] parts = Data.Split(':');
            if (parts.Length < 1)
                throw new ArgumentException("Invalid LoginResponse format.");

            IsSuccess = parts[0].Equals("SUCCESS");
            Token = parts.Length > 1 ? parts[1] : null;
        }
    }
}
