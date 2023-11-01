using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Database
{
    public class NpgsqlDatabaseManager : DatabaseConnection, IDatabaseManager
    {
        private readonly AppDbContext _dbContext;

        public NpgsqlDatabaseManager(IConfiguration configuration, AppDbContext dbContext) : base(configuration)
        {
            _dbContext = dbContext;
        }

        public bool CheckLogin(string username, string password)
        {
            return _dbContext.Users.Any(u => u.Username == username && u.Password == password);
        }

        public void InitializeDatabase()
        {
            try
            {
                // Vérifie si la base de données existe
                if (_dbContext.Database.CanConnect())
                {
                    // Applique les migrations si elles n'ont pas été appliquées
                    _dbContext.Database.Migrate();
                    Log.Information("Base de données initialisée.");
                }
                else
                {
                    Log.Error("Impossible de se connecter à la base de données.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de l'initialisation de la base de données.");
            }
        }

        public void StoreTokenInDatabase(string username, string token)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.Token = token;
                _dbContext.SaveChanges();
            }
        }
    }
}
