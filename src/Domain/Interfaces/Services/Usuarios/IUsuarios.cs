using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.Usuarios
{
    public interface IUsuarios
    {
        public Task<int> AddUsuario(Usuario usuario);
        public Task<Usuario> GetUsuarioByUsername(string username);
    }
}
