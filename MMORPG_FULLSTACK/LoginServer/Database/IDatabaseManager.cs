using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Database
{
    public interface IDatabaseManager
    {
        void InitializeDatabase();
        bool CheckLogin(string username, string password);
        void StoreTokenInDatabase(string username, string token);
    }
}
