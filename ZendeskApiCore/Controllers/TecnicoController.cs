using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TecnicoController(ESCORIALContext context) : ControllerBase
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
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tecnico>>> GetTecnico()
        {
            var tecnicos = await context.Tecnicos.ToListAsync();
            if (tecnicos is null || tecnicos.IsNullOrEmpty())
                return NotFound();
            return Ok(tecnicos);
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
        /// <response code="400">Error en la solicitud enviada.</response>
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Tecnico>> GetProblema(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();
            var tecnico = await context.Tecnicos.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (tecnico is null)
                return NotFound();
            return Ok(tecnico);
        }
    }
}