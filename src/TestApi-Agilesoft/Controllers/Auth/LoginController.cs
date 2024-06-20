using Domain.Entities;
using Domain.Interfaces.Services.Auth;
using Domain.Interfaces.Services.Usuarios;
using Infraestructure.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Api.Auth;
using Models.DTO.Domain;
using Models.DTO.Infraestructure;
using System.Security.Claims;


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
        /// Método que envía el token válido para el uso de la API.
        /// </summary>
        /// <param name="loginRequest">Objeto de solicitud de inicio de sesión que contiene las credenciales del usuario.</param>
        /// <returns>Retorna el nombre del usuario y el token de acceso a la aplicación.</returns>
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var UserName = loginRequest.UserName;

                if (loginRequest == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Usuario y/o password incorrectos.");

                if (string.IsNullOrEmpty(UserName))
                    return StatusCode(StatusCodes.Status400BadRequest, "Usuario y/o password incorrectos.");

                if (String.IsNullOrEmpty(loginRequest.Password))
                    return StatusCode(StatusCodes.Status500InternalServerError, "Usuario y/o password incorrectos.");
                

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

                LoginResponse loginResponse = new()
                {
                    Username = usuario.Username,
                    AccessToken = generateToken.Access_Token

                };

                return StatusCode(StatusCodes.Status200OK, loginResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Método para registrar un nuevo usuario.
        /// </summary>
        /// <param name="loginRequest">Objeto de solicitud de registro que contiene la información del nuevo usuario.</param>
        /// <returns>Resultado de la operación de registro.</returns>
        [Route("SignUp")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var UserName = loginRequest.UserName;

                if (loginRequest == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Usuario y/o password incorrectos.");
                }

                if (string.IsNullOrEmpty(UserName))
                    return StatusCode(StatusCodes.Status400BadRequest, "Usuario y/o password incorrectos.");

                if (string.IsNullOrEmpty(loginRequest.Password))
                    return StatusCode(StatusCodes.Status400BadRequest, "Usuario y/o password incorrectos.");

                var usuario = await _usuario.GetUsuarioByUsername(UserName).ConfigureAwait(false);

                if (usuario != null)
                    return StatusCode(StatusCodes.Status400BadRequest, "El nombre de usuario ya existe");

                //registrar usuario

                Usuario usuarioRegistro = new Usuario()
                {
                    Username = loginRequest.UserName.ToUpper(),
                    Password = loginRequest.Password,
                    Nombre = loginRequest.Nombre.ToUpper()
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

        /// <summary>
        /// Obtiene la información del usuario autenticado.
        /// </summary>
        /// <returns>Retorna la información del usuario autenticado.</returns>
        [Route("GetUserInfo")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {

                // Recupera los claims del usuario autenticado
                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (username == null)
                    return StatusCode(StatusCodes.Status401Unauthorized, "Ocurrio un error al verificar el username, si el error persiste contacta con un usuario");

                //obtiene al usuario
                var usuario = await _usuario.GetUsuarioByUsername(username).ConfigureAwait(false);

                if(usuario == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "El usuario no existe, vuelva intentarlo mas tarde");

                //crea el objeto respuesta
                UserInfoResponse userInfoResponse = new()
                {
                    Nombre = usuario.Nombre,
                    Username = usuario.Username
                };

                return StatusCode(StatusCodes.Status200OK, userInfoResponse);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
