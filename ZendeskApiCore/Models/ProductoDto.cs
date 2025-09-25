#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace ZendeskApiCore.Models;

public class ProductoDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Descripcion { get; set; }
    [NotMapped]
    public TipoProducto TipoProducto { get; set; }
    [NotMapped]
    public Rubro Rubro { get; set; }
}