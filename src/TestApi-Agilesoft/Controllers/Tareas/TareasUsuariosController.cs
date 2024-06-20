using Domain.Entities;
using Domain.Interfaces.Services.TareasUsuarios;
using Domain.Interfaces.Services.Usuarios;
using Domain.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Api.Tareas;

namespace TestApi_Agilesoft.Controllers.Tareas
{
    [ApiController]
    [Route("TestApi/UserTasks")]
    [Consumes("application/json")]
    [Authorize]
    public class TareasUsuariosController : Controller
    {

        private readonly ITareasGenerales _tareasGenerales;
        private readonly ITareasUsuario _tareasUsuarios;
        private readonly IUsuarios _usuarios;

        public TareasUsuariosController(ITareasGenerales tareasGenerales, ITareasUsuario tareasUsuarios, IUsuarios usuarios)
        {
            _tareasGenerales = tareasGenerales;
            _tareasUsuarios = tareasUsuarios;
            _usuarios = usuarios;
        }



        /// <summary>
        /// Obtiene una lista de las tareas asociadas a un usuario en específico.
        /// </summary>
        /// <param name="UsuarioId">ID del usuario.</param>
        /// <returns>Una lista de las tareas asociadas al usuario.</returns>
        [Route("ListTareas")]
        [HttpGet]
        public async Task<IActionResult> ListTareasUsuario(int UsuarioId)
        {

            try
            {
                if (UsuarioId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, si vuelve a ocurrir contacte con un administrador");



                var specTareaUser = new TareasUsuarioSpecifications(UsuarioId);
                var listTareas = await _tareasUsuarios.GetTareasByUserId(specTareaUser).ConfigureAwait(false);

                if (listTareas == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error al consultar sus tareas, si vuelve a ocurrir contacte con un administrador");

                var tareasUsuarioIds = listTareas.Select(x => x.TareasId).ToList();

                var listTareasUsuarios = await _tareasGenerales.GetTareasGeneralesByIds(tareasUsuarioIds).ConfigureAwait(false);

                if(listTareasUsuarios == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error al consultar las tareas generales, si vuelve a ocurrir contacte con un administrador");



                return StatusCode(StatusCodes.Status200OK, listTareasUsuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Desvincula una tarea de un usuario.
        /// </summary>
        /// <param name="tareasUsuarioRequest">Objeto de solicitud con la información necesaria para desvincular la tarea del usuario.</param>
        /// <returns>Un mensaje de respuesta indicando si la desvinculación fue exitosa.</returns>
        [Route("DeleteTareaUsuario")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTareaUsuario([FromBody] TareasUsuarioRequest tareasUsuarioRequest)
        {

            try
            {
                if (tareasUsuarioRequest == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, si vuelve a ocurrir contacte con un administrador");

                if (tareasUsuarioRequest.UsuarioId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, si vuelve a ocurrir contacte con un administrador");

                if (tareasUsuarioRequest.TareasId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, si vuelve a ocurrir contacte con un administrador");

                var specTareaUser = new TareasUsuarioSpecifications(tareasUsuarioRequest.UsuarioId, tareasUsuarioRequest.TareasId);
                var TareaUsuario = await _tareasUsuarios.GetTareasByUserIdAndTaskId(specTareaUser).ConfigureAwait(false);

                var tareaGeneral = await _tareasGenerales.GetByTareasGeneralesId(tareasUsuarioRequest.TareasId).ConfigureAwait(false);

                if(tareaGeneral == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error al consultar la tarea, si vuelve a ocurrir contacte con un administrador");

                if (TareaUsuario == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error al consultar la tarea del usuario, si vuelve a ocurrir contacte con un administrador");

                var usuario = await _usuarios.GetUsuarioById(tareasUsuarioRequest.UsuarioId).ConfigureAwait(false);

                if (usuario == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "El usuario al que intenta vincular la tarea no existe, vuelva a intentarlo mas tarde");

                var resultTareaUsuario = await _tareasUsuarios.DeleteTareaUsuario(TareaUsuario).ConfigureAwait(false);

                if (resultTareaUsuario <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error al desvincular la tarea del usuario, si vuelve a ocurrir contacte con un administrador");



                return StatusCode(StatusCodes.Status200OK, "La tarea " + tareaGeneral.Nombre + " se desvinculo con exito al usuario " + usuario.Nombre);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Vincula una tarea a un usuario.
        /// </summary>
        /// <param name="tareasUsuarioRequest">Objeto de solicitud con la información necesaria para vincular la tarea al usuario.</param>
        /// <returns>Un mensaje de respuesta indicando si la vinculación fue exitosa.</returns>
        [Route("AddTareaUsuario")]
        [HttpPost]
        public async Task<IActionResult> AddTareaUsuario([FromBody] TareasUsuarioRequest tareasUsuarioRequest)
        {

            try
            {
                if (tareasUsuarioRequest == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, si vuelve a ocurrir contacte con un administrador");

                if (tareasUsuarioRequest.UsuarioId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, si vuelve a ocurrir contacte con un administrador");

                if (tareasUsuarioRequest.TareasId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, si vuelve a ocurrir contacte con un administrador");

                var specTareaUser = new TareasUsuarioSpecifications(tareasUsuarioRequest.UsuarioId, tareasUsuarioRequest.TareasId);
                var TareaUsuario = await _tareasUsuarios.GetTareasByUserIdAndTaskId(specTareaUser).ConfigureAwait(false);

                if (TareaUsuario != null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, la tarea ya se encuentra vinculada al usuario");

                var tareaGeneral = await _tareasGenerales.GetByTareasGeneralesId(tareasUsuarioRequest.TareasId).ConfigureAwait(false);

                if (tareaGeneral == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error al consultar la tarea, si vuelve a ocurrir contacte con un administrador");


                var usuario = await _usuarios.GetUsuarioById(tareasUsuarioRequest.UsuarioId).ConfigureAwait(false);

                if (usuario == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "El usuario al que intenta vincular la tarea no existe, vuelva a intentarlo mas tarde");

                var addTareaUsuario = new TareasUsuario()
                {
                    UsuarioId = tareasUsuarioRequest.UsuarioId,
                    TareasId = tareasUsuarioRequest.TareasId
                };

                var resultTareaUsuario = await _tareasUsuarios.AddTareaUsuario(addTareaUsuario).ConfigureAwait(false);

                if (resultTareaUsuario <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error al vincular la tarea al usuario, si vuelve a ocurrir contacte con un administrador");



                return StatusCode(StatusCodes.Status200OK, "La tarea " + tareaGeneral.Nombre + " se vinculo con exito al usuario " + usuario.Nombre);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
