using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Interfaces.Auth
{
    public interface IEncriptar
    {
        void Encriptar(string password);
        string ObtenerSalt();
        string ObtenerHash();

        /// <summary>
        /// Recibe password sin cifrar y hash almacenadas para comparar.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns>Regresa true si el hash generado es valido, false de lo contrario</returns>
        bool VerificarHash(string password, string hashAlmacenada);
    }
}
