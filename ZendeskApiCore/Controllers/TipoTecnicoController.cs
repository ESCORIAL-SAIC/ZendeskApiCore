using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoTecnicoController(ESCORIALContext context, ILogger<LoginController> logger) : ControllerBase
    {

        // GET: api/TipoTecnico
        /// <summary>
        /// Obtiene los tipos de técnico.
        /// </summary>
        /// <remarks>
        /// Requiere autenticación. Nivel usuario.
        /// </remarks>
        /// <returns>Una colección de tipos de técnicos.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Devuelve el listado de objetos solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoTecnico>>> GetTipoTecnico()
        {
            try
            {
                var tiposTecnico = await context.TiposTecnico.ToListAsync();
                if (tiposTecnico is null || tiposTecnico.IsNullOrEmpty())
                    return NotFound();
                return Ok(tiposTecnico);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método GetTipoTecnico");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }

        // GET: api/TipoTecnico/5
        /// <summary>
        /// Obtiene un tipo de técnico específico por su ID.
        /// </summary>
        /// <remarks>
        /// Requiere autenticación. Nivel usuario.
        /// </remarks>
        /// <param name="id">ID del tipo de técnico.</param>
        /// <returns>El tipo de técnico encontrado.</returns>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="400">BadRequest. Error en la solicitud enviada.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoTecnico>> GetTipoTecnico(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("No se proporcionó un ID válido.");
                var tipoTecnico = await context.TiposTecnico.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (tipoTecnico == null)
                    return NotFound();
                return Ok(tipoTecnico);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método GetTipoTecnico(id)");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }
    }
}
