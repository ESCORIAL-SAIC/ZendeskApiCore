using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    /// <summary>
    /// API Para manejo de reclamos en Zendesk desde Web.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReclamoWebZendeskController(ESCORIALContext context, IMapper mapper) : ControllerBase
    {
        private readonly ESCORIALContext _context = context;
        private readonly IMapper _mapper = mapper;

        // GET: api/ReclamoWebZendesk
        /// <summary>
        /// Obtiene todos los reclamos web desde Zendesk.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <returns>Una colección de reclamos web.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Devuelve el listado de objetos solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReclamoWebZendesk>>> GetReclamoWebZendesk()
        {
            var reclamos = await _context.ReclamosWebZendesk.ToListAsync();
            foreach (var reclamo in reclamos)
            {
                var itemsReclamo = await _context.ItemsReclamoWebZendesk.Where(x => x.ReclamoId.Equals(reclamo.Id.ToString())).ToListAsync();
                reclamo.ItemsReclamoWebZendesk = itemsReclamo;
            }
            return reclamos;
        }

        // GET: api/ReclamoWebZendesk/5
        /// <summary>
        /// Obtiene un reclamo web específico por su ID.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <param name="id">ID del reclamo web.</param>
        /// <returns>El reclamo web encontrado.</returns>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReclamoWebZendesk>> GetReclamoWebZendesk(int? id)
        {
            var reclamoWebZendesk = await _context.ReclamosWebZendesk.FindAsync(id);

            if (reclamoWebZendesk == null)
            {
                return NotFound();
            }

            reclamoWebZendesk.ItemsReclamoWebZendesk = await _context.ItemsReclamoWebZendesk.Where(x => x.ReclamoId.Equals(reclamoWebZendesk.Id.ToString())).ToListAsync();

            return reclamoWebZendesk;
        }

        // PUT: api/ReclamoWebZendesk/5
        /// <summary>
        /// Actualiza un reclamo web existente en Zendesk.
        /// </summary>
        /// <remarks>Requiere autencicación. Nivel adminsitrador.</remarks>
        /// <param name="id">ID del reclamo web a actualizar.</param>
        /// <param name="reclamoWebZendeskDto">Datos del reclamo web actualizado.</param>
        /// <returns>Respuesta HTTP indicando el resultado de la actualización.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="204">NoContent. Objeto correctamente actualizado en la BD.</response>
        /// <response code="400">BadRequest. No se ha actualizado el objeto en la BD. Formato del objeto incorrecto o Id inexistente.</response>
        /// <response code="404">NotFound. No se encontró el reclamo a actualizar.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReclamoWebZendesk(int? id, [FromBody] ReclamoWebZendeskDto reclamoWebZendeskDto)
        {
            if (id == null)
                return BadRequest("ID no proporcionado.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reclamoWebZendeskInDb = await _context.ReclamosWebZendesk
                .FirstOrDefaultAsync(r => r.Id == id);
            if (reclamoWebZendeskInDb == null)
                return NotFound();
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _mapper.Map(reclamoWebZendeskDto, reclamoWebZendeskInDb);
                    _context.Entry(reclamoWebZendeskInDb).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync();

                    if (!ReclamoWebZendeskExists(id.Value))
                        return NotFound();
                    throw;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/ReclamoWebZendesk
        /// <summary>
        /// Crea un nuevo reclamo web en Zendesk.
        /// </summary>
        /// <remarks>
        /// Requiere autenticación. Nivel administrador.
        /// En este método se utiliza un DTO reducido para facilitar la carga de datos. La información no incluída es generada o asignada automáticamente, o es para uso interno del sistema.
        /// </remarks>
        /// <param name="reclamoWebZendeskDto">Datos del nuevo reclamo web.</param>
        /// <returns>Respuesta HTTP con el reclamo web creado.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="201">Created. Objeto correctamente creado en la BD.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        /// <response code="500">InternalServerError. Se produjo un error al crear el reclamo.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpPost]
        public async Task<ActionResult<ReclamoWebZendesk>> PostReclamoWebZendesk([FromBody] ReclamoWebZendeskDto reclamoWebZendeskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reclamoWebZendesk = _mapper.Map<ReclamoWebZendesk>(reclamoWebZendeskDto);
            reclamoWebZendesk.CreatedAt = DateTime.Now;
            reclamoWebZendesk.FechaHoraIngresoPagina = DateTime.Now;
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.ReclamosWebZendesk.Add(reclamoWebZendesk);
                    await _context.SaveChangesAsync();
                    foreach (var item in reclamoWebZendesk.ItemsReclamoWebZendesk)
                    {
                        item.ReclamoId = reclamoWebZendesk.Id.ToString();
                        _context.ItemsReclamoWebZendesk.Add(item);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error al crear el reclamo.\nMessage: {ex.Message}\nStack trace: {ex.StackTrace}\nInner exception: {ex.InnerException}");
                }
            }
            return CreatedAtAction("GetReclamoWebZendesk", new { id = reclamoWebZendesk.Id }, reclamoWebZendesk);
        }

        // DELETE: api/ReclamoWebZendesk/5
        /// <summary>
        /// Elimina un reclamo web existente en Zendesk por su ID.
        /// </summary>
        /// <remarks>
        /// Requiere autenticación. Nivel administrador.
        /// En este método se utiliza un DTO reducido para facilitar la carga de datos. La información no incluída es generada o asignada automáticamente, o es para uso interno del sistema.
        /// </remarks>
        /// <param name="id">ID del reclamo web a eliminar.</param>
        /// <returns>Respuesta HTTP indicando el resultado de la eliminación.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="204">NoContent. Objeto correctamente eliminado de la BD.</response>
        /// <response code="400">BadRequest. No se ha eliminado el objeto en la BD. Id incorrecto.</response>
        /// <response code="404">NotFound. No se encontró el elemento en la BD.</response>
        /// <response code="500">InternalServerError. Ocurrió un error al intentar eliminar el reclamo.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReclamoWebZendesk(int? id)
        {
            if (id == null)
            {
                return BadRequest("ID no proporcionado.");
            }
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var reclamoWebZendesk = await _context.ReclamosWebZendesk.FindAsync(id);
                if (reclamoWebZendesk == null)
                {
                    return NotFound();
                }
                var itemsReclamoWebZendesk = await _context.ItemsReclamoWebZendesk
                    .Where(x => x.ReclamoId.Equals(reclamoWebZendesk.Id.ToString()))
                    .ToListAsync();
                _context.ReclamosWebZendesk.Remove(reclamoWebZendesk);
                foreach (var item in itemsReclamoWebZendesk)
                {
                    _context.ItemsReclamoWebZendesk.Remove(item);
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return NoContent();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al eliminar el reclamo.");
            }
        }

        /// <summary>
        /// Verifica si existe un reclamo web en Zendesk por su ID.
        /// </summary>
        /// <param name="id">ID del reclamo web a verificar.</param>
        /// <returns>True si el reclamo web existe; false en caso contrario.</returns>
        private bool ReclamoWebZendeskExists(int? id)
        {
            return _context.ReclamosWebZendesk.Any(e => e.Id == id);
        }
    }
}
