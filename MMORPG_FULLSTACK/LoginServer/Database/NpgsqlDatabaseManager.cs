using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Database
{
    public class NpgsqlDatabaseManager : DatabaseConnection, IDatabaseManager
    {
        public NpgsqlDatabaseManager(IConfiguration configuration) : base(configuration) { }

        public bool CheckLogin(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void InitializeDatabase()
        {
            throw new NotImplementedException();
        }

        public void StoreTokenInDatabase(string username, string token)
        {
            throw new NotImplementedException();
        }

    }
}
