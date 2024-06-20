using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Api.Auth
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public string AccessToken { get; set; }
    }
}
