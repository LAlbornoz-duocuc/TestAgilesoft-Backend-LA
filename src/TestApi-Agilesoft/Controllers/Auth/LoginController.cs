using Domain.Entities;
using Domain.Interfaces.Services.Auth;
using Domain.Interfaces.Services.Usuarios;
using Infraestructure.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Api.Auth;
using Models.DTO.Domain;
using Models.DTO.Infraestructure;


namespace TestApi_Agilesoft.Controllers.Auth
{
    [ApiController]
    [Route("TestApi/Auth")]
    [Consumes("application/json")]
    public class LoginController : Controller
    {
        private readonly ITokenJWT _jwt;
        private readonly ILogin _loginService;
        private readonly IUsuarios _usuario;

        public LoginController(ITokenJWT jwt, ILogin loginService, IUsuarios usuario)
        {
            _jwt = jwt;
            _loginService = loginService;
            _usuario = usuario;
        }

        /// <summary>
        /// Metodo que envia el token valido para el uso de la API
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var UserName = loginRequest.UserName;

                if (loginRequest == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Usuario y/o password incorrectos.");
                }

                if (String.IsNullOrEmpty(loginRequest.Password))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Usuario y/o password incorrectos.");
                }

                var ValidarCredenciales = await _loginService.ValidarCredenciales(new UsuarioLogin(UserName, loginRequest.Password)).ConfigureAwait(false);

                if (!ValidarCredenciales)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Usuario y/o password incorrectos.");
                }

                var usuario = await _usuario.GetUsuarioByUsername(UserName).ConfigureAwait(false);

                if (usuario == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "El usuario consultado no existe");



                UsuarioForJWT usuarioForJWT = new UsuarioForJWT
                {
                    Id = usuario.Id,
                    Nombre = usuario.Username
                };

                var generateToken = _jwt.GenerateToken(usuarioForJWT);



                return StatusCode(StatusCodes.Status200OK, generateToken.Access_Token);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var UserName = loginRequest.UserName;

                if (loginRequest == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Usuario y/o password incorrectos.");
                }

                if (String.IsNullOrEmpty(loginRequest.Password))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Usuario y/o password incorrectos.");
                }

                var usuario = await _usuario.GetUsuarioByUsername(UserName).ConfigureAwait(false);

                if (usuario != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "El nombre de usuario ya existe");

                //registrar usuario

                Usuario usuarioRegistro = new Usuario()
                {
                    Username = loginRequest.UserName,
                    Password = loginRequest.Password
                };

                var resultUsuarioRegistro = await _usuario.AddUsuario(usuarioRegistro).ConfigureAwait(false);

                if (resultUsuarioRegistro <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error al crear el usuario, contactese con un administrador");



                return StatusCode(StatusCodes.Status200OK, "El usuario se registro correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
