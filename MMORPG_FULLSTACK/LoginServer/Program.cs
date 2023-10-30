using LoginServer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static void Main(string[] args)
        {
            // Chargement de la configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json");

            Configuration = builder.Build();

            // Configuration des services
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Création du fournisseur de services
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Exécution du serveur de connexion
            Console.WriteLine("Starting login server...");
            var server = serviceProvider.GetRequiredService<LoginServer>();
            server.Start();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Récupérez directement l'objet DatabaseSettings depuis la configuration
            var dbSettings = Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();

            // Configuration de la base de données
            if (dbSettings.Type == "Npgsql")
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(dbSettings.ConnectionString));
            }
            else if (dbSettings.Type == "Sqlite")
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite(dbSettings.ConnectionString));
            }

            // Configuration d'autres services
            services.AddTransient<LoginServer>();

            // Appliquer les migrations
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }
        }

    }
}
