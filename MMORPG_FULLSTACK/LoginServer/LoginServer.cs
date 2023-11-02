using System;
using System.Net;
using System.Net.Sockets;
using Npgsql;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using LoginServer.Database;
using LoginServer.Packets;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace LoginServer
{
    public class LoginServer
    {
        private TcpListener listener;
        private int port;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
        // Dictionnaire pour stocker les jetons des sessions et les associer aux noms d'utilisateur
        private Dictionary<string, string> userTokens = new Dictionary<string, string>();
        private Dictionary<TcpClient, string> connectedClientsWithTokens = new Dictionary<TcpClient, string>();
        public bool IsMaintenanceMode { get; private set; } = false;

        public LoginServer(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            port = _configuration.GetValue<int>("LoginServerPort");
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            listener.Start();
            Log.Information("Login server started on port {0}", port);

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
                Log.Information("Client connected from {0}", client.Client.RemoteEndPoint);
                connectedClientsWithTokens.Add(client, "");

                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        Log.Information("Client disconnected");
                        return;
                    }

                    Packet receivedPacket = new Packet(PacketType.LoginRequest, "");
                    receivedPacket.Deserialize(buffer.Take(bytesRead).ToArray());

                    if (receivedPacket.Type == PacketType.LoginRequest)
                    {
                        LoginRequest loginPacket = new LoginRequest("", "");
                        loginPacket.Deserialize(buffer.Take(bytesRead).ToArray());
                        Log.Information("Received login request from {0}", loginPacket.Username);

                        bool loginSuccess = CheckLogin(loginPacket.Username, loginPacket.Password);

                        if (loginSuccess)
                        {
                            string token = Guid.NewGuid().ToString();
                            userTokens[loginPacket.Username] = token;

                            var user = _dbContext.Users.FirstOrDefault(u => u.Username == loginPacket.Username);
                            if (user != null)
                            {
                                user.Token = token;
                                await _dbContext.SaveChangesAsync();
                                Log.Information("Token {0} saved for {1}", token, loginPacket.Username);
                            }

                            LoginResponse responsePacket = new LoginResponse(true, token);
                            byte[] response = responsePacket.Serialize();
                            await stream.WriteAsync(response, 0, response.Length);
                            Log.Information("Sent login response to {0}", loginPacket.Username);
                            connectedClientsWithTokens[client] = token;
                        }
                        else
                        {
                            Log.Information("Login failed for {0}", loginPacket.Username);
                            // Envoyer une notification au client
                            Notification notificationPacket = new Notification("Login ou mot de passe incorrecte");
                            byte[] notificationData = notificationPacket.Serialize();
                            await stream.WriteAsync(notificationData, 0, notificationData.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while handling client");
            }
            finally
            {
                client.Close();
                connectedClientsWithTokens.Remove(client);
                Log.Information("Client disconnected");
            }
        }
        private bool CheckLogin(string username, string password)
        {
            return _dbContext.Users.Any(u => u.Username == username && u.Password == password);
        }

        private bool HasAuthorityA(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                // Utilisateur non trouvé
                return false;
            }

            return user.Authority.Contains("A") && user.IsValid && !user.IsBanned;
        }

        public async void SetMaintenanceMode(bool isMaintenance)
        {
            IsMaintenanceMode = isMaintenance;
            Log.Information("This server is now in maintenance mode: {0}", isMaintenance);
            if (isMaintenance)
            {
                // Créer le paquet de maintenance
                Maintenance maintenancePacket = new Maintenance(true, "Server is under maintenance");

                // Sérialiser le paquet en tableau d'octets pour l'envoi
                byte[] data = maintenancePacket.Serialize();

                foreach (var clientTokenPair in connectedClientsWithTokens)
                {
                    if (clientTokenPair.Key.Connected && !HasAuthorityA(GetUsernameByToken(clientTokenPair.Value)))
                    {
                        using (var stream = clientTokenPair.Key.GetStream())
                        {
                            await stream.WriteAsync(data, 0, data.Length);
                        }
                    }
                }
            }
            else
            {
                Log.Information("Server is no longer in maintenance mode");
            }
        }

        private string GetUsernameByToken(string token)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Token == token);
            return user?.Username;
        }
    }
}
