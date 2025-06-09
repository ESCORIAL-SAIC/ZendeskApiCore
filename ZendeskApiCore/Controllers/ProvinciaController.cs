using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinciaController(ESCORIALContext context, ILogger<LoginController> logger) : ControllerBase
    {

        // GET: api/Provincia
        /// <summary>
        /// Obtiene las provincias a cargar en reclamos.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <returns>Una colección de provincias.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Devuelve el listado de objetos solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="404">NotFound. No se encontró el objeto solicitado.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [HttpGet]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<IEnumerable<Provincia>>> GetProvincia()
        {
            try
            {
                var provincias = await context.Provincias.ToListAsync();
                if (provincias is null || provincias.IsNullOrEmpty())
                    return NotFound();
                return Ok(provincias);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método GetProvincia");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }

        // GET: api/Provincia/5
        /// <summary>
        /// Obtiene una provincia específica por su ID.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <param name="id">ID de la provincia.</param>
        /// <returns>La provincia encontrada.</returns>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="400">BadRequest. Error en la solicitud enviada.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [HttpGet("{id}")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<Provincia>> GetProvincia(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("No se proporcionó un ID válido.");
                var provincia = await context.Provincias.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (provincia is null)
                    return NotFound();
                return Ok(provincia);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método GetProvincia(id)");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }
    }
}
