using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Interfaces.Auth
{
    public interface IGenerarHash
    {
        string GenerarHash(string password, string salt);
        bool VerificarHash(string password, string hashAlmacenada);
    }
}
