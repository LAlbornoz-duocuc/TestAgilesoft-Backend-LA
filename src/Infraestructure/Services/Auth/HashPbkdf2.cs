using Infraestructure.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Services.Auth
{
    public class HashPbkdf2 : IGenerarHash
    {
        const int iteraciones = 10000;
        const int hashLargo = 20;
        public string GenerarHash(string password, string salt)
        {

            byte[] saltBytes = Convert.FromBase64String(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password: password, salt: saltBytes, iterations: iteraciones);

            byte[] hash = pbkdf2.GetBytes(hashLargo);

            /*
             * Se usan 36 bytes: 20 para hash y 16 para salt
             */
            byte[] hashBytes = new byte[36];

            Array.Copy(saltBytes, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, hashLargo);

            return Convert.ToBase64String(hashBytes);

        }

        public bool VerificarHash(string password, string hashAlmacenada)
        {
            if (string.IsNullOrEmpty(hashAlmacenada))
            {
                return false;
            }

            byte[] hashBytes = Convert.FromBase64String(hashAlmacenada);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password: password, salt: salt, iterations: iteraciones);
            byte[] hash = pbkdf2.GetBytes(hashLargo);

            for (int i = 0; i < hashLargo; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;

        }
    }
}
