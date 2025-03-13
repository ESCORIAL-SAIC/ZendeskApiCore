using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinciaController(ESCORIALContext context) : ControllerBase
    {
        private readonly ESCORIALContext _context = context;

        // GET: api/Provincia
        /// <summary>
        /// Obtiene las provincias a cargar en reclamos.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <returns>Una colección de provincias.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el listado de objetos solicitado.</response>   
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>   
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Provincia>>> GetProvincia()
        {
            var provincias = await _context.Provincias.ToListAsync();
            return provincias;
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
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Provincia>> GetProvincia(Guid? id)
        {
            var provincia = await _context.Provincias.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (provincia == null)
            {
                return NotFound();
            }
            return provincia;
        }
    }
}
