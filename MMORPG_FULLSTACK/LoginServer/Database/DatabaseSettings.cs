using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Database
{
    public class DatabaseSettings
    {
        public string Type { get; set; }
        public string ConnectionString { get; set; }
        public int Port { get; set; } // Vous n'avez pas utilisé ce port pour le moment.
    }
}
