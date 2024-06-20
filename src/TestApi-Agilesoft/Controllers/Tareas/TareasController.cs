using Domain.Entities;
using Domain.Interfaces.Services.TareasUsuarios;
using Infraestructure.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Api.Tareas;
using Models.DTO.Domain;
using Models.DTO.Infraestructure;

namespace TestApi_Agilesoft.Controllers.Tareas
{
    [ApiController]
    [Route("TestApi/Tasks")]
    [Consumes("application/json")]
    [Authorize]
    public class TareasController : Controller
    {
        private readonly ITareasGenerales _tareasGenerales;

        public TareasController(ITareasGenerales tareasGenerales)
        {
            _tareasGenerales = tareasGenerales;
        }

        /// <summary>
        /// Obtiene la lista de todas las tareas.
        /// </summary>
        /// <returns>Un listado de tareas.</returns>
        [Route("ListTareas")]
        [HttpGet]
        public async Task<IActionResult> ListTareas()
        {
            try
            {
                var listTareas = await _tareasGenerales.GetTareasGenerales().ConfigureAwait(false);

                return StatusCode(StatusCodes.Status200OK, listTareas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Agrega una nueva tarea.
        /// </summary>
        /// <param name="tareasGeneralesDTO">Objeto DTO con la información de la tarea a agregar.</param>
        /// <returns>Resultado de la operación de agregar la tarea.</returns>
        [Route("AddTareas")]
        [HttpPost]
        public async Task<IActionResult> AddTareas([FromBody] TareasGeneralesDTO tareasGeneralesDTO)
        {
            try
            {
                if (tareasGeneralesDTO == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, si vuelve a ocurrir contacte con un administrador");


                if (tareasGeneralesDTO.Id > 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, La tarea que esta intentando crear tiene problemas en los datos, verique y vuelva a intentarlo");


                TareasGenerales tareasGenerales = new()
                {
                    Nombre = tareasGeneralesDTO.Nombre,
                    Descripcion = tareasGeneralesDTO.Descripcion,
                    Estado = tareasGeneralesDTO.Estado,
                    FechaCreacion = DateTime.Now,
                    FechaUltimaActualizacion = DateTime.Now
                };

                var TareaResult = await _tareasGenerales.AddTarea(tareasGenerales).ConfigureAwait(false);

                if (TareaResult <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error al crear la tarea, si vuelve a ocurrir contacte con un administrador");


                return StatusCode(StatusCodes.Status200OK, "La tarea se creo con exito");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza la información de una tarea existente.
        /// </summary>
        /// <param name="tareasGeneralesDTO">Objeto DTO con la información actualizada de la tarea.</param>
        /// <returns>Resultado de la operación de actualización de la tarea.</returns>
        [Route("UpdateTareas")]
        [HttpPut]
        public async Task<IActionResult> UpdateTareas([FromBody] TareasGeneralesDTO tareasGeneralesDTO)
        {
            try
            {
                if (tareasGeneralesDTO == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, si vuelve a ocurrir contacte con un administrador");


                if (tareasGeneralesDTO.Id <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, La tarea que esta intentando actualizar tiene problemas en los datos, verique y vuelva a intentarlo");

                var tareasGenerales = await _tareasGenerales.GetByTareasGeneralesId(tareasGeneralesDTO.Id).ConfigureAwait(false);
                
                tareasGenerales.Nombre = tareasGeneralesDTO.Nombre;
                tareasGenerales.Descripcion = tareasGeneralesDTO.Descripcion;
                tareasGenerales.Estado = tareasGeneralesDTO.Estado;
                tareasGenerales.FechaUltimaActualizacion = DateTime.Now;



                var TareaResult = await _tareasGenerales.UpdateTarea(tareasGenerales).ConfigureAwait(false);

                if (TareaResult <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error al actualizar la tarea, si vuelve a ocurrir contacte con un administrador");


                return StatusCode(StatusCodes.Status200OK, "La tarea se actualizo con exito");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Actualiza el estado de una tarea específica.
        /// </summary>
        /// <param name="tareasGeneralesRequest">Objeto de solicitud con la información necesaria para actualizar el estado de la tarea.</param>
        /// <returns>Resultado de la operación de actualización del estado de la tarea.</returns>
        [Route("UpdateEstado")]
        [HttpPatch]
        public async Task<IActionResult> UpdateEstado([FromBody] TareasGeneralesRequest tareasGeneralesRequest)
        {
            try
            {
                if (tareasGeneralesRequest == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, si vuelve a ocurrir contacte con un administrador");


                if (tareasGeneralesRequest.Id <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error, La tarea que esta intentando actualizar tiene problemas en los datos, verique y vuelva a intentarlo");


                var tareasGenerales = await _tareasGenerales.GetByTareasGeneralesId(tareasGeneralesRequest.Id).ConfigureAwait(false);

                tareasGenerales.Estado = tareasGeneralesRequest.Estado;


                var TareaResult = await _tareasGenerales.UpdateTarea(tareasGenerales).ConfigureAwait(false);

                if (TareaResult <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, "Ocurrio un error al actualizar la tarea, si vuelve a ocurrir contacte con un administrador");


                return StatusCode(StatusCodes.Status200OK, "La tarea se creo con exito");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
