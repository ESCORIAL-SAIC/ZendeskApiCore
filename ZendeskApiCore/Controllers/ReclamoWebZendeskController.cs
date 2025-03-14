using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        // GET: api/ReclamoWebZendesk
        /// <summary>
        /// Obtiene todos los reclamos web desde Zendesk.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <returns>Una colección de reclamos web.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Devuelve el listado de objetos solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="404">NotFound. No se encontró el objeto solicitado.</response>
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReclamoWebZendesk>>> GetReclamoWebZendesk()
        {
            var reclamos = await context.ReclamosWebZendesk.ToListAsync();
            if (reclamos is null || reclamos.IsNullOrEmpty())
                return NotFound();
            foreach (var reclamo in reclamos)
            {
                var itemsReclamo = await context.ItemsReclamoWebZendesk.Where(x => x.ReclamoId.Equals(reclamo.Id.ToString())).ToListAsync();
                reclamo.ItemsReclamoWebZendesk = itemsReclamo;
            }
            return Ok(reclamos);
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
        /// <response code="400">BadRequest. Error en la solicitud enviada.</response>
        [Authorize(Policy = "RequireUserRole")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReclamoWebZendesk>> GetReclamoWebZendesk(int id)
        {
            if (id < 1)
                return BadRequest();

            var reclamoWebZendesk = await context.ReclamosWebZendesk.FindAsync(id);

            if (reclamoWebZendesk is null)
                return NotFound();

            reclamoWebZendesk.ItemsReclamoWebZendesk = await context.ItemsReclamoWebZendesk.Where(x => x.ReclamoId.Equals(reclamoWebZendesk.Id.ToString())).ToListAsync();

            return Ok(reclamoWebZendesk);
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
        public async Task<IActionResult> PutReclamoWebZendesk(int id, [FromBody] ReclamoWebZendeskDto reclamoWebZendeskDto)
        {
            if (id < 1)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reclamoWebZendeskInDb = await context.ReclamosWebZendesk
                .FirstOrDefaultAsync(r => r.Id == id);
            if (reclamoWebZendeskInDb is null)
                return NotFound();
            await using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    mapper.Map(reclamoWebZendeskDto, reclamoWebZendeskInDb);
                    context.Entry(reclamoWebZendeskInDb).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync();

                    if (!ReclamoWebZendeskExists(id))
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
            var reclamoWebZendesk = mapper.Map<ReclamoWebZendesk>(reclamoWebZendeskDto);
            reclamoWebZendesk.CreatedAt = DateTime.Now;
            reclamoWebZendesk.FechaHoraIngresoPagina = DateTime.Now;
            await using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    context.ReclamosWebZendesk.Add(reclamoWebZendesk);
                    await context.SaveChangesAsync();
                    foreach (var item in reclamoWebZendesk.ItemsReclamoWebZendesk)
                    {
                        item.ReclamoId = reclamoWebZendesk.Id.ToString();
                        context.ItemsReclamoWebZendesk.Add(item);
                    }
                    await context.SaveChangesAsync();
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
        public async Task<IActionResult> DeleteReclamoWebZendesk(int id)
        {
            if (id < 1)
                return BadRequest();
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var reclamoWebZendesk = await context.ReclamosWebZendesk.FindAsync(id);
                if (reclamoWebZendesk is null)
                    return NotFound();
                var itemsReclamoWebZendesk = await context.ItemsReclamoWebZendesk
                    .Where(x => x.ReclamoId.Equals(reclamoWebZendesk.Id.ToString()))
                    .ToListAsync();
                context.ReclamosWebZendesk.Remove(reclamoWebZendesk);
                foreach (var item in itemsReclamoWebZendesk)
                    context.ItemsReclamoWebZendesk.Remove(item);
                await context.SaveChangesAsync();
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
            return context.ReclamosWebZendesk.Any(e => e.Id == id);
        }
    }
}
