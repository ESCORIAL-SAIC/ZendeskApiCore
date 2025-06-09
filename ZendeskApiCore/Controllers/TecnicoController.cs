using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TecnicoController(ESCORIALContext context, ILogger<LoginController> logger) : ControllerBase
    {

        // GET: api/Tecnico
        /// <summary>
        /// Obtiene los técnicos.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <returns>Una colección de técnicos.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Devuelve el listado de objetos solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="404">NotFound. No se encontró el objeto solicitado.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [HttpGet]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<IEnumerable<Tecnico>>> GetTecnico()
        {
            try
            {
                var tecnicos = await context.Tecnicos.ToListAsync();
                if (tecnicos is null || tecnicos.IsNullOrEmpty())
                    return NotFound();
                return Ok(tecnicos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método GetTecnico");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }

        // GET: api/Tecnico/5
        /// <summary>
        /// Obtiene un técnico específico por su ID.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <param name="id">ID del técnico.</param>
        /// <returns>El problema encontrado.</returns>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="400">BadRequest. Error en la solicitud enviada.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Tecnico>> GetProblema(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("No se proporcionó un ID válido.");
                var tecnico = await context.Tecnicos.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (tecnico is null)
                    return NotFound();
                return Ok(tecnico);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método GetTecnico(id)");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }
    }
}