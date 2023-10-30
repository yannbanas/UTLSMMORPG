using LoginServer.Database;
using LoginServer.Packets;
using System.Net.Sockets;

namespace TestLoginServer
{
    public class UnitTest1
    {
        private readonly string _connectionString = "Host=192.168.1.48;Username=btechpostgresuser;Password=2B90F460-D040-4A11-9582-AF56C5598C18;Database=btechlogindb";

        [Fact]
        public void TestDatabaseAndServerConnection()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // 1. Vérifiez la connexion à Postgres.
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_connectionString)
                .Options;
            using var context = new AppDbContext(options);

            // Testez la connexion à la base de données en récupérant un enregistrement.
            var anyUser = context.Users.FirstOrDefault();

            // 2. Créez un utilisateur de test.
            var user = new User
            {
                Username = "testUser",
                Password = "testPassword",
                Token = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                IsValid = true
            };
            context.Users.Add(user);
            context.SaveChanges();

            // 3. Simulez une connexion au serveur et envoi/réception de paquets.
            bool serverConnected = SimulateServerConnectionWithPacketExchange();
            Assert.True(serverConnected);

            // Nettoyer: supprimer l'utilisateur de test de la base de données
            context.Users.Remove(user);
            context.SaveChanges();
        }


        // Cette méthode établit une véritable connexion TCP au port 8888, envoie un paquet et attend une réponse.
        private bool SimulateServerConnectionWithPacketExchange()
        {
            using TcpClient client = new TcpClient();
            try
            {
                client.Connect("192.168.1.48", 8888);

                var loginRequest = new LoginRequest("testUser", "testPassword");
                var serializedRequest = loginRequest.Serialize();

                client.GetStream().Write(serializedRequest, 0, serializedRequest.Length);

                byte[] buffer = new byte[4096];
                int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);

                var responsePacket = new LoginResponse(false);
                responsePacket.Deserialize(buffer);

                Console.WriteLine(responsePacket.ToString()); // Affichage du contenu du paquet de réponse

                return responsePacket != null;
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }
}