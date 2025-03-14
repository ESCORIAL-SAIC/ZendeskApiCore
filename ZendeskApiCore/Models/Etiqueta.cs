# nullable disable

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ZendeskApiCore.Models
{
    [Keyless]
    public class Etiqueta
    {
        [Column("numero")]
        public int Numero { get; set; }
        [JsonIgnore]
        [Column("producto_id")]
        public Guid ProductoId { get; set; }
        [JsonIgnore]
        [Column("producto_c")]
        public string ProductoCodigo { get; set; }
        [JsonIgnore]
        [Column("producto_n")]
        public string ProductoNombre { get; set; }
        [JsonIgnore]
        [Column("tipo")]
        public string ProductoTipoCodigo { get; set; }
        [NotMapped]
        public ProductoDto Producto { get; set; }
    }
}
