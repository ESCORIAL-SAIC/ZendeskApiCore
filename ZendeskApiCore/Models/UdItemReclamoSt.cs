#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZendeskApiCore.Models
{
    [Table("ud_item_reclamo_st")]
    public class UdItemReclamoSt
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("bo_owner_id")]
        public Guid BoOwnerId { get; set; }
        [Column("numeroserie")]
        public string NumeroSerie { get; set; } 
    }
}
