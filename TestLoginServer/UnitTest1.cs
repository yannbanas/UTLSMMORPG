using LoginServer.Database;
using LoginServer.Packets;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;

namespace TestLoginServer
{
    public class UnitTest1
    {
        private readonly string _connectionString = "Host=192.168.1.48;Username=btechpostgresuser;Password=2B90F460-D040-4A11-9582-AF56C5598C18;Database=btechlogindb";


        //=======================================================================================================//
        //====================================== Authentification  ==============================================//
        //=======================================================================================================//


        /// <summary>
        /// this scenario tests the connection to the database and connection to the server and sending/receiving packets loginRequest and loginResponse
        /// send a loginRequest packet with a valid username and password
        /// after the test remove the user from the database
        /// </summary>
        [Fact]
        public void ScenarioOne()
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
                Created=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Token =Guid.NewGuid().ToString(),

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

        /// <summary>
        /// this scenario tests the connection to the database and connection to the server and sending/receiving packets loginRequest and loginResponse
        /// send a loginRequest packet with a unvalid username and password
        /// after the test remove the user from the database
        /// </summary>
        [Fact]
        public void ScenarioTwo()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_connectionString)
                .Options;
            using var context = new AppDbContext(options);

            var anyUser = context.Users.FirstOrDefault();

            var user = new User
            {
                Username = "sdbfhgdefoyg",
                Password = "passwordOne",
                Created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Token = Guid.NewGuid().ToString(),
            };
            context.Users.Add(user);
            context.SaveChanges();

            bool serverConnected = SimulateServerConnectionWithWrongLogin();
            Assert.True(serverConnected);

            context.Users.Remove(user);
            context.SaveChanges();
        }

        [Fact]
        public void CreateNewUserInDB()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_connectionString)
                .Options;
            using var context = new AppDbContext(options);

            var anyUser = context.Users.FirstOrDefault();

            var user = new User
            {
                Username = "kqsdjfijkj",
                Password = "ksjdfijdefijepiofj546165",
                Created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Token = Guid.NewGuid().ToString(),
            };
            context.Users.Add(user);
            context.SaveChanges();
        }

        [Fact]
        public void UpdateSpecificUserInDB()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // 1. Connect to Postgres.
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_connectionString)
                .Options;

            using var context = new AppDbContext(options);

            // 2. Fetch a user with a specific username.
            var userToUpdate = context.Users.FirstOrDefault(u => u.Username == "kqsdjfijkj"); // Replace "testUser" with the desired username.

            if (userToUpdate != null)
            {
                // 3. Update the password of that user.
                userToUpdate.Password = "PasswordUpdatedByXunit"; // Replace "newPassword" with the desired password.

                // 4. Save the changes.
                context.SaveChanges();
            }
        }

        [Fact]
        public void UpdateAuthorityOfSpecificUserInDB()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_connectionString)
                .Options;

            using var context = new AppDbContext(options);


            var userToUpdate = context.Users.FirstOrDefault(u => u.Username == "kqsdjfijkj"); // Replace "testUser" with the desired username.

            if (userToUpdate != null)
            {
                // update authority of that user to admin
                userToUpdate.Authority = "A"; 

                context.SaveChanges();
            }
        }

        [Fact]
        public void DeleteSpecificUserInDB()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_connectionString)
                .Options;

            using var context = new AppDbContext(options);

            var userToDelete = context.Users.FirstOrDefault(u => u.Username == "kqsdjfijkj");

            if (userToDelete != null)
            {
                context.Users.Remove(userToDelete);
                context.SaveChanges();
            }
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

        private bool SimulateServerConnectionWithWrongLogin()
        {
            using TcpClient client = new TcpClient();
            try
            {
                client.Connect("192.168.1.48", 8888);

                var loginRequest = new LoginRequest("sdbfhgdefoyg", "Azerty*1");
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