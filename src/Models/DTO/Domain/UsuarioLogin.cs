using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Domain
{
    public class UsuarioLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UsuarioLogin(string userName, string password)
        {
            Username = userName;
            Password = password;
        }
    }
}
