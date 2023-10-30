using LoginServer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
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
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss}|{SourceContext}|{Message:lj}|{NewLine}{Exception}")
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day,
                              outputTemplate: "{Timestamp:HH:mm:ss}|{SourceContext}|{Message:lj}|{NewLine}{Exception}")
                .CreateLogger();

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
            Log.Information("Starting login server...");
            var server = serviceProvider.GetRequiredService<LoginServer>();
            server.Start();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            Log.Information("Configuring services...");
            services.AddSingleton<IConfiguration>(Configuration);

            // Récupérez directement l'objet DatabaseSettings depuis la configuration
            var dbSettings = Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
            Log.Information("Database settings: {0}", dbSettings.ToString());

            // Configuration de la base de données
            if (dbSettings.Type == "Npgsql")
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(dbSettings.ConnectionString));
                Log.Information("Using PostgreSQL database");
            }
            else if (dbSettings.Type == "Sqlite")
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite(dbSettings.ConnectionString));
                Log.Information("Using SQLite database");
            }

            // Configuration d'autres services
            services.AddTransient<LoginServer>();

        }

    }
}
