using AutoMapper;
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
    }
}
