using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        //P = Player, G = GameMaster, A = Admin
        public string Authority { get; set; } = "P";
        public bool IsValid { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastConnectionTime { get; set; }
    }
}
