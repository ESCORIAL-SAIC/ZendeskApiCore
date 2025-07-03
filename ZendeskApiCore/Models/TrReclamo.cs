#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZendeskApiCore.Models
{
    [Table("trreclamo")]
    public class TrReclamo
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("estado")]
        [StringLength(1)]
        public string Estado { get; set; }
        [Column("numerodocumento")]
        [StringLength(20)]
        public string NumeroDocumento { get; set; }
        [Column("nombre")]
        [StringLength(60)]
        public string Nombre { get; set; }
        [Column("flag_id")]
        public Guid FlagId { get; set; }

    }
}
