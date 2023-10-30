using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LoginServer.Database
{
    public abstract class DatabaseConnection
    {
        protected readonly AppDbContext _context;

        protected DatabaseConnection(IConfiguration configuration)
        {
            var dbType = configuration["DatabaseSettings:Type"];
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            if (dbType == "Npgsql")
            {
                optionsBuilder.UseNpgsql(configuration["DatabaseSettings:ConnectionString"]);
            }
            else if (dbType == "SQLite")
            {
                optionsBuilder.UseSqlite(configuration["DatabaseSettings:ConnectionString"]);
            }
            else
            {
                throw new Exception("Invalid database type in configuration.");
            }

            _context = new AppDbContext(optionsBuilder.Options);
        }
    }
}
