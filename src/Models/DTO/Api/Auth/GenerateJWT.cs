using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Api.Auth
{
    public class GenerateJWT
    {
        public string Access_Token { get; set; }
        public int UserName { get; set; }
        public DateTime FechaExp { get; set; }
    }
}
