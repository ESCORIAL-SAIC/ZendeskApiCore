using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemaController(ESCORIALContext context) : ControllerBase
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
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Problema>>> GetProblema()
        {
            var problemas = await context.Problemas.ToListAsync();
            if (problemas == null || problemas.IsNullOrEmpty())
                return NotFound();
            return problemas;
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
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Problema>> GetProblema(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();
            var problema = await context.Problemas.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (problema == null)
                return NotFound();
            return problema;
        }
    }
}
