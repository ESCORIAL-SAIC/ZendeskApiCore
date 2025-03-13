using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZendeskApiCore.Models;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoProductoController : ControllerBase
    {
        private readonly ESCORIALContext _context;

        public TipoProductoController(ESCORIALContext context)
        {
            _context = context;
        }

        // GET: api/TipoProducto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoProducto>>> GetTiposProducto()
        {
            return await _context.TiposProducto.ToListAsync();
        }

        // GET: api/TipoProducto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoProducto>> GetTipoProducto(Guid id)
        {
            var tipoProducto = await _context.TiposProducto.FindAsync(id);

            if (tipoProducto == null)
            {
                return NotFound();
            }

            return tipoProducto;
        }
    }
}
