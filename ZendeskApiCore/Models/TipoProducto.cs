#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZendeskApiCore.Models;

public class TipoProducto
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("codigo")]
    [StringLength(100)]
    public string Codigo { get; set; }
    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; }  
}