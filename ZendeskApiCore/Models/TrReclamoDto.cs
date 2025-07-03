#nullable disable
namespace ZendeskApiCore.Models
{
    public class TrReclamoDto
    {
        public string Nombre  { get; set; }
        public string NumeroDocumento { get; set; }
        public string Estado { get; set; }
        public Flag Flag { get; set; }
    }
}
