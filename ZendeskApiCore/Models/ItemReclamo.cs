#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZendeskApiCore.Models
{
    [Table("itemreclamo")]
    public class ItemReclamo
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("tipotransaccion_id")]
        public Guid TipoTransaccionId { get; set; }
        [Column("placeowner_id")]
        public Guid PlaceOwnerId { get; set; }
        [Column("numerodocumento")]
        public string NumeroDocumento { get; set; }
        [Column("nombretr")]
        public string NombreTr { get; set; }
    }
}
