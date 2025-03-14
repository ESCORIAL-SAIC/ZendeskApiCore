using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ESCORIALContext _context;
        private readonly IMapper _mapper;

        public ProductoController(ESCORIALContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Producto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
        {
            var productos = await _context.Productos
                .Where(producto => producto.ActiveStatus == 0)
                .Join(_context.Segmentos,
                    producto => producto.SegmentoId,
                    segmento => segmento.Id,
                    (producto, segmento) => new { producto, segmento })
                .Join(_context.TiposProducto,
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
            return productos;
        }

        // GET: api/Producto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(Guid id)
        {
            var producto = await _context.Productos
                .Where(producto => producto.ActiveStatus == 0
                                   && producto.Id == id)
                .Join(_context.Segmentos,
                    producto => producto.SegmentoId,
                    segmento => segmento.Id,
                    (producto, segmento) => new { producto, segmento })
                .Join(_context.TiposProducto,
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
            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        //GET: api/producto/tipo/5
        [HttpGet("tipo/{idTipo}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductoTipo(Guid idTipo)
        {
            var productos = await _context.Productos
                .Where(producto => producto.ActiveStatus == 0)
                .Join(_context.Segmentos,
                    producto => producto.SegmentoId,
                    segmento => segmento.Id,
                    (producto, segmento) => new { producto, segmento })
                .Join(_context.TiposProducto,
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
            return productos;
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
        [HttpPost]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<Etiqueta>> PostEtiqueta([FromBody] EtiquetaDto etiquetaDto)
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
            var etiqueta = await _context.Etiquetas
                .FirstOrDefaultAsync(e => e.Numero == etiquetaDto.Numero && e.ProductoTipoCodigo == codigoProducto);
            if (etiqueta is null)
                return NotFound();
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == etiqueta.ProductoId);
            if (producto is null)
                return NotFound();
            etiqueta.Producto = _mapper.Map<ProductoDto>(producto);
            etiqueta.Producto.TipoProducto = await _context.TiposProducto
                .FirstOrDefaultAsync(p => p.Codigo == etiquetaDto.ProductoTipo.Codigo);
            if (etiqueta.Producto.TipoProducto is null)
                return NotFound();
            return Ok(etiqueta);
        }
    }
}
