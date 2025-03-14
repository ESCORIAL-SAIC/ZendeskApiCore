using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoProductoController(ESCORIALContext context) : ControllerBase
    {

        // GET: api/TipoProducto
        /// <summary>
        /// Obtiene los tipos de producto.
        /// </summary>
        /// <remarks>Requiere autenticaci�n. Nivel usuario.</remarks>
        /// <returns>Una colecci�n de productos.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Devuelve el listado de objetos solicitado.</response>
        /// <response code="403">Forbidden. Autorizaci�n denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="404">NotFound. No se encontr� el objeto solicitado.</response>
        [HttpGet]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<IEnumerable<TipoProducto>>> GetTiposProducto()
        {
            var tipos = await context.TiposProducto.ToListAsync();
            if (tipos is null || tipos.IsNullOrEmpty())
                return NotFound();
            return Ok(tipos);
        }

        // GET: api/TipoProducto/5
        /// <summary>
        /// Obtiene un tipo de producto espec�fico por su ID.
        /// </summary>
        /// <remarks>Requiere autenticaci�n. Nivel usuario.</remarks>
        /// <param name="id">ID del tipo de producto.</param>
        /// <returns>El problema encontrado.</returns>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="403">Forbidden. Autorizaci�n denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="400">BadRequest. Error en la solicitud enviada.</response>
        [HttpGet("{id}")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<TipoProducto>> GetTipoProducto(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();
            var tipoProducto = await context.TiposProducto.FindAsync(id);
            if (tipoProducto == null)
                return NotFound();
            return Ok(tipoProducto);
        }
    }
}
