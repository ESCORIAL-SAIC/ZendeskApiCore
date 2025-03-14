# nullable disable

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZendeskApiCore.Models
{
    [Keyless]
    public class Etiqueta
    {
        [Column("numero")]
        public int Numero { get; set; }
        [Column("producto_id")]
        public Guid ProductoId { get; set; }
        [Column("producto_c")]
        public string ProductoCodigo { get; set; }
        [Column("producto_n")]
        public string ProductoNombre { get; set; }
        [Column("tipo")]
        public string ProductoTipo { get; set; }
    }
}
