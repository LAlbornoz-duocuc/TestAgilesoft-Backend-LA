using Infraestructure.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Services.Auth
{
    public class EncriptarPbkdf2 : IEncriptar
    {
        private readonly IGenerarHash _generarHash;
        private readonly IGenerarSalt _generarSalt;
        private string _hashGenerado;
        private string _saltGenerado;
        public EncriptarPbkdf2(IGenerarHash generarHash, IGenerarSalt generarSalt)
        {
            _generarHash = generarHash;
            _generarSalt = generarSalt;
        }
        public void Encriptar(string password)
        {
            _saltGenerado = _generarSalt.GenerarSalt();
            _hashGenerado = _generarHash.GenerarHash(password: password, salt: _saltGenerado);
        }

        public string ObtenerHash()
        {
            return _hashGenerado;
        }

        public string ObtenerSalt()
        {
            return _saltGenerado;
        }

        public bool VerificarHash(string password, string hashAlmacenada)
        {
            return _generarHash.VerificarHash(password, hashAlmacenada);
        }
    }
}
