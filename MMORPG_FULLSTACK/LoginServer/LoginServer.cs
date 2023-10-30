using System;
using System.Net;
using System.Net.Sockets;
using Npgsql;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoginServer.Database;
using LoginServer.Packets;

namespace LoginServer
{
    public class LoginServer
    {
        private TcpListener listener;
        private readonly AppDbContext _dbContext;

        // Dictionnaire pour stocker les jetons des sessions et les associer aux noms d'utilisateur
        private Dictionary<string, string> userTokens = new Dictionary<string, string>();

        public LoginServer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            listener = new TcpListener(IPAddress.Any, 8888); // Vous pouvez rendre le port configurable plus tard
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine("Login server started...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Task.Run(() => HandleClient(client));
            }
        }

        private async void HandleClient(TcpClient client)
        {
            try
            {
                Console.WriteLine("Client connecté.");

                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        Console.WriteLine("Le client s'est déconnecté avant d'envoyer des données.");
                        return;
                    }

                    Packet receivedPacket = new Packet(PacketType.LoginRequest, "");
                    receivedPacket.Deserialize(buffer.Take(bytesRead).ToArray());

                    if (receivedPacket.Type == PacketType.LoginRequest)
                    {
                        LoginRequest loginPacket = new LoginRequest("", "");
                        loginPacket.Deserialize(buffer.Take(bytesRead).ToArray());

                        bool loginSuccess = CheckLogin(loginPacket.Username, loginPacket.Password);
                        LoginResponse responsePacket;

                        if (loginSuccess)
                        {
                            string token = Guid.NewGuid().ToString();
                            userTokens[loginPacket.Username] = token;

                            var user = _dbContext.Users.FirstOrDefault(u => u.Username == loginPacket.Username);
                            if (user != null)
                            {
                                user.Token = token;
                                await _dbContext.SaveChangesAsync();
                            }

                            responsePacket = new LoginResponse(true, token);
                        }
                        else
                        {
                            responsePacket = new LoginResponse(false);
                        }

                        byte[] response = responsePacket.Serialize();
                        await stream.WriteAsync(response, 0, response.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors du traitement du client : " + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
        private bool CheckLogin(string username, string password)
        {
            return _dbContext.Users.Any(u => u.Username == username && u.Password == password);
        }

    }
}
