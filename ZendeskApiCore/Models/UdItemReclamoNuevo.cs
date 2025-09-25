#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZendeskApiCore.Models
{
    [Table("ud_itemreclamo_nuevo")]
    public class UdItemReclamoNuevo
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("bo_owner_id")]
        public Guid BoOwnerId { get; set; }
        [Column("numeroserie")]
        public int NumeroSerie { get; set; } 
    }
}
