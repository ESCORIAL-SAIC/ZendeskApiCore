using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController(ESCORIALContext context, IMapper mapper, ILogger<LoginController> logger) : ControllerBase
    {

        // GET: api/Producto
        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <returns>Un listado de productos.</returns>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="400">BadRequest. Error en la solicitud.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [HttpGet]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
        {
            try
            {
                var productos = await context.Productos
                .Where(producto => producto.ActiveStatus == 0)
                .Join(context.Segmentos,
                    producto => producto.SegmentoId,
                    segmento => segmento.Id,
                    (producto, segmento) => new { producto, segmento })
                .Join(context.TiposProducto,
                    productoSegmento => productoSegmento.segmento.Segmento1Id,
                    tipoProducto => tipoProducto.Id,
                    (productoSegmento, tipoProducto) => new { productoSegmento, tipoProducto })
                .Select(productoSegmentoTipo =>
                    new ProductoDto
                    {
                        Id = productoSegmentoTipo.productoSegmento.producto.Id,
                        Codigo = productoSegmentoTipo.productoSegmento.producto.Codigo,
                        Descripcion = productoSegmentoTipo.productoSegmento.producto.Descripcion,
                        TipoProducto = productoSegmentoTipo.tipoProducto,
                    })
                .ToListAsync();
                if (productos is null || productos.IsNullOrEmpty())
                    return NotFound();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método GetProductos");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }

        // GET: api/Producto/5
        /// <summary>
        /// Obtiene un producto a partir de su ID.
        /// </summary>
        /// <param name="id">ID del producto a buscar.</param>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <returns>Un listado de productos.</returns>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="400">BadRequest. Error en la solicitud.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [HttpGet("{id}")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<ProductoDto>> GetProducto(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("No se proporcionó un ID válido.");
                var producto = await context.Productos
                    .Where(producto => producto.ActiveStatus == 0
                                       && producto.Id == id)
                    .Join(context.Segmentos,
                        producto => producto.SegmentoId,
                        segmento => segmento.Id,
                        (producto, segmento) => new { producto, segmento })
                    .Join(context.TiposProducto,
                        productoSegmento => productoSegmento.segmento.Segmento1Id,
                        tipoProducto => tipoProducto.Id,
                        (productoSegmento, tipoProducto) => new { productoSegmento, tipoProducto })
                    .Select(productoSegmentoTipo =>
                        new ProductoDto
                        {
                            Id = productoSegmentoTipo.productoSegmento.producto.Id,
                            Codigo = productoSegmentoTipo.productoSegmento.producto.Codigo,
                            Descripcion = productoSegmentoTipo.productoSegmento.producto.Descripcion,
                            TipoProducto = productoSegmentoTipo.tipoProducto,
                        })
                    .FirstOrDefaultAsync();
                if (producto is null)
                    return NotFound();
                return Ok(producto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método GetProductos(id)");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }

        //GET: api/producto/tipo/5
        /// <summary>
        /// Obtiene un listado de productos por tipo.
        /// </summary>
        /// <param name="idTipo">ID del tipo de producto a buscar.</param>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <returns>Un listado de productos.</returns>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="400">BadRequest. Error en la solicitud.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [HttpGet("tipo/{idTipo}")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductoTipo(Guid idTipo)
        {
            try
            {
                if (idTipo == Guid.Empty)
                    return BadRequest("No se proporcionó un ID válido.");
                var productos = await context.Productos
                    .Where(producto => producto.ActiveStatus == 0)
                    .Join(context.Segmentos,
                        producto => producto.SegmentoId,
                        segmento => segmento.Id,
                        (producto, segmento) => new { producto, segmento })
                    .Join(context.TiposProducto,
                        productoSegmento => productoSegmento.segmento.Segmento1Id,
                        tipoProducto => tipoProducto.Id,
                        (productoSegmento, tipoProducto) => new { productoSegmento, tipoProducto })
                    .Select(productoSegmentoTipo =>
                        new ProductoDto
                        {
                            Id = productoSegmentoTipo.productoSegmento.producto.Id,
                            Codigo = productoSegmentoTipo.productoSegmento.producto.Codigo,
                            Descripcion = productoSegmentoTipo.productoSegmento.producto.Descripcion,
                            TipoProducto = productoSegmentoTipo.tipoProducto,
                        })
                    .Where(producto => producto.TipoProducto.Id == idTipo)
                    .ToListAsync();
                if (productos is null || productos.IsNullOrEmpty())
                    return NotFound();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método GetProductoTipo(id)");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }

        //POST: api/producto
        /// <summary>
        /// Obtiene un producto a partir de su número de serie y tipo.
        /// </summary>
        /// <remarks>Requiere autenticación. Nivel usuario.</remarks>
        /// <param name="etiquetaDto">DTO.</param>
        /// <returns>El producto encontrado.</returns>
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        /// <response code="403">Forbidden. Autorización denegada. No cuenta con los permisos suficientes.</response>
        /// <response code="400">BadRequest. Error en los datos enviados.</response>
        /// <response code="500">InternalServerError. Error interno del servidor. Comunicarse con sistemas.</response>
        [HttpPost]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<Etiqueta>> PostEtiqueta([FromBody] EtiquetaDto etiquetaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(etiquetaDto);
                if (etiquetaDto.Numero < 1)
                    return BadRequest("Numero de serie inválido");
                if (string.IsNullOrEmpty(etiquetaDto.ProductoTipo.Codigo))
                    return BadRequest("Tipo de producto no válido");
                string codigoProducto;
                var cocinaValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "C", "Cocina", "27d3f815-3f17-43bd-b1e9-e5c366276f13" };
                var termoValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "CA", "Calefon", "T", "Termo", "7aaadd97-8a7d-4e74-97bb-4378890ee1f0", "9d1ab5c1-9a9b-4ea5-8885-f255afb68d0f" };
                if (cocinaValues.Contains(etiquetaDto.ProductoTipo.Codigo))
                    codigoProducto = "COCINA";
                else if (termoValues.Contains(etiquetaDto.ProductoTipo.Codigo))
                    codigoProducto = "TERMOTANQUE";
                else
                    return BadRequest("Tipo de producto incorrecto.");
                var etiqueta = await context.Etiquetas
                    .FirstOrDefaultAsync(e => e.Numero == etiquetaDto.Numero && e.ProductoTipoCodigo == codigoProducto);
                if (etiqueta is null)
                    return NotFound();
                var producto = await context.Productos
                    .FirstOrDefaultAsync(p => p.Id == etiqueta.ProductoId);
                if (producto is null)
                    return NotFound();
                etiqueta.Producto = mapper.Map<ProductoDto>(producto);
                etiqueta.Producto.TipoProducto = await context.TiposProducto
                    .FirstOrDefaultAsync(p => p.Codigo == etiquetaDto.ProductoTipo.Codigo);
                var trReclamoDto = context.ItemsReclamo
                    .GroupJoin(
                        context.TrReclamos,
                        item => item.PlaceOwnerId,
                        reclamo => reclamo.Id,
                        (item, reclamos) => new { item, reclamos }
                    )
                    .SelectMany(
                        x => x.reclamos.DefaultIfEmpty(),
                        (x, reclamo) => new { x.item, reclamo }
                    )
                    .GroupJoin(
                        context.UdItemsReclamoSt,
                        x => x.item.Id,
                        ud => ud.BoOwnerId,
                        (x, uds) => new { x.item, x.reclamo, uds }
                    )
                    .SelectMany(
                        x => x.uds.DefaultIfEmpty(),
                        (x, udItemReclamoSt) => new { x.item, x.reclamo, udItemReclamoSt }
                    )
                    .Where(x =>
                        x.reclamo != null &&
                        x.reclamo.BoPlaceId == new Guid("28295D31-34DE-441D-B329-DB9EDC4828A9") &&
                        x.item.TipoTransaccionId == new Guid("9917E6DE-2C20-408C-AA20-C4B183BDAED2") &&
                        x.udItemReclamoSt != null &&
                        x.udItemReclamoSt.NumeroSerie == etiqueta.Numero.ToString());
                if (etiqueta.Producto.TipoProducto is null)
                    return NotFound();
                return Ok(etiqueta);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el método PostEtiqueta(etiquetaDto)");
                return StatusCode(500, "Ocurrió un error inesperado. Contacte a sistemas.");
            }
        }
        
    }
}
