#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZendeskApiCore.Models
{
    [Table("rubro")]
    public class Rubro
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("nombrerubro")]
        public string Nombre { get; set; }
    }
}
