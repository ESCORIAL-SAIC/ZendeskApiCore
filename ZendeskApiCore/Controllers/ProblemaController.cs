using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemaController(ESCORIALContext context, ILogger<LoginController> logger) : ControllerBase
    {
        // GET: api/Problema
        /// <summary>
        /// Obtiene los posibles problemas a cargar en reclamos.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <returns>Una colección de problemas.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Devuelve el listado de objetos solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="404">NotFound. Objeto no encontrado.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [HttpGet]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<IEnumerable<Problema>>> GetProblema()
        {
            try
            {
                var problemas = await context.Problemas.ToListAsync();
                if (problemas == null || problemas.IsNullOrEmpty())
                    return NotFound();
                return Ok(problemas);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método GetProblema");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }

        // GET: api/Problema/5
        /// <summary>
        /// Obtiene un problema específico por su ID.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <param name="id">ID del problema.</param>
        /// <returns>El problema encontrado.</returns>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="400">BadRequest. Error en la solicitud.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [HttpGet("{id}")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<Problema>> GetProblema(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("No se proporcionó un ID válido.");
                var problema = await context.Problemas.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (problema == null)
                    return NotFound();
                return Ok(problema);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método GetProblema(id)");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }
    }
}
