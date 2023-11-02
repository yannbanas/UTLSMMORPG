using System;
using System.Text.RegularExpressions;

namespace LoginServer.Packets
{
    public class LoginRequest : Packet
    {


        public string Username { get; private set; }
        public string Password { get; private set; }

        public LoginRequest(string username, string password) : base(PacketType.LoginRequest, $"{username}:{password}")
        {

            Username = username;
            Password = password;
        }

        public override void Deserialize(byte[] data)
        {
            base.Deserialize(data); // Appel de la méthode Deserialize de Packet

            string[] parts = Data.Split(':');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid LoginRequest format.");

            Username = parts[0];
            Password = parts[1];

        }

    }
}
