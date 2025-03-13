using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoTecnicoController(ESCORIALContext context) : ControllerBase
    {
        private readonly ESCORIALContext _context = context;

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
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoTecnico>>> GetTipoTecnico()
        {
            var tiposTecnico = await _context.TiposTecnico.ToListAsync();
            return tiposTecnico;
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
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoTecnico>> GetTipoTecnico(Guid? id)
        {
            var tipoTecnico = await _context.TiposTecnico.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (tipoTecnico == null)
            {
                return NotFound();
            }
            return tipoTecnico;
        }
    }
}
