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
        public string Token { get; set; } = Guid.NewGuid().ToString();
        //P = Player, G = GameMaster, A = Admin
        public string Authority { get; set; } = "P";
        public bool IsValid { get; set; } = true;
        public bool IsBanned { get; set; } = false;
        public string Created { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public string? LastConnectionTime { get; set; } = "";

        public override string ToString()
        {
            return $"Id: {Id}, Username: {Username}, Password: {Password}, Token: {Token}, Authority: {Authority}, IsValid: {IsValid}, IsBanned: {IsBanned}, Created: {Created}, LastConnectionTime: {LastConnectionTime}";
        }
    }
}
