using Infraestructure.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Services.Auth
{
    public class SaltPbkdf2 : IGenerarSalt
    {
        public string GenerarSalt()
        {
            byte[] salt = new byte[16];
            RandomNumberGenerator.Create().GetBytes(salt);
            //new RNGCryptoServiceProvider().GetBytes(salt);
            return Convert.ToBase64String(salt);
        }
    }
}
