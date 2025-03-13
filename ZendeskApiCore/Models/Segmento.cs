#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZendeskApiCore.Models;

[Table("segmento")]
public class Segmento
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("bo_place_id")]
    public Guid BoPlaceId { get; set; }
    [Column("bo_owner_id")]
    public Guid BoOwnerId { get; set; }
    [Column("segmento1_id")]
    public Guid Segmento1Id { get; set; }
    [Column("segmento2_id")]
    public Guid Segmento2Id { get; set; }
    [Column("segmento3_id")]
    public Guid Segmento3Id { get; set; }
    [Column("segmento4_id")]
    public Guid Segmento4Id { get; set; }
    [Column("segmento5_id")]
    public Guid Segmento5Id { get; set; }
    [Column("segmento6_id")]
    public Guid Segmento6Id { get; set; }
    [Column("segmento7_id")]
    public Guid Segmento7Id { get; set; }
    [Column("segmento8_id")]
    public Guid Segmento8Id { get; set; }
    [Column("segmento9_id")]
    public Guid Segmento9Id { get; set; }
    [Column("segmento10_id")]
    public Guid Segmento10Id { get; set; }
    [Column("nombre1")]
    [StringLength(100)]
    public string Nombre1 { get; set; }
    [Column("nombre2")]
    [StringLength(100)]
    public string Nombre2 { get; set; }
    [Column("nombre3")]
    [StringLength(100)]
    public string Nombre3 { get; set; }
    [Column("nombre4")]
    [StringLength(100)]
    public string Nombre4 { get; set; }
    [Column("nombre5")]
    [StringLength(100)]
    public string Nombre5 { get; set; }
    [Column("nombre6")]
    [StringLength(100)]
    public string Nombre6 { get; set; }
    [Column("nombre7")]
    [StringLength(100)]
    public string Nombre7 { get; set; }
    [Column("nombre8")]
    [StringLength(100)]
    public string Nombre8 { get; set; }
    [Column("nombre9")]
    [StringLength(100)]
    public string Nombre9 { get; set; }
    [Column("nombre10")]
    [StringLength(100)]
    public string Nombre10 { get; set; }
}