#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace ZendeskApiCore.Models
{
    public class TrReclamoDto
    {
        public string Nombre  { get; set; }
        public string NumeroDocumento { get; set; }
        public string Estado { get; set; }
        [NotMapped]
        public Flag Flag { get; set; }
    }
}
