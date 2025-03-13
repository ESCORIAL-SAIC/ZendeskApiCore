#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZendeskApiCore.Models;

[Table("itemtipoclasificador")]
public class ItemTipoClasificador
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("bo_place_id")]
    public Guid BoPlaceId { get; set; }
    [Column("bo_owner_id")]
    public Guid BoOwnerId { get; set; }
    [Column("activestatus")]
    public int ActiveStatus { get; set; }
    [Column("codigo")]
    [StringLength(60)]
    public string Codigo { get; set; }
    [Column("nombre")]
    [StringLength(120)]
    public string Nombre { get; set; }
    [Column("place_owner_id")]
    public Guid PlaceOwnerId { get; set; }
    [Column("referencia_id")]
    public Guid ReferenceId { get; set; }
    [Column("boextension_id")]
    public Guid BoExtensionId { get; set; }
    [Column("bocsf_id")]
    public Guid BoCsfId { get; set; }
}