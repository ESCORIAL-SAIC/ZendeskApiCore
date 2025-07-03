#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZendeskApiCore.Models
{
    [Table("flag")]
    public partial class Flag
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("descripcion")]
        [StringLength(100)]
        public string Descripcion { get; set; }
        [Column("codigo")]
        [StringLength(20)]
        public string Codigo { get; set; }
    }
}
