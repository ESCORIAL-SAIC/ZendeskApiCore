namespace ZendeskApiCore.Models;

/// <summary>
/// DTO para carga de datos de reclamo web en Zendesk.
/// </summary>
public partial class ReclamoWebZendeskDto
{
    /// <summary>
    /// Apellido y nombre del reclamante.
    /// </summary>
    public string? ApellidoNombre { get; set; }
    /// <summary>
    /// DNI del reclamante.
    /// </summary>
    public string? Dni { get; set; }
    /// <summary>
    /// Dirección del reclamante.
    /// </summary>
    public string? Direccion { get; set; }
    /// <summary>
    /// Entre calles de la dirección cargada.
    /// </summary>
    public string? EntreCalles { get; set; }
    /// <summary>
    /// Id correspondiente a la localidad de residencia del reclamante.
    /// </summary>
    public string? LocalidadId { get; set; }
    /// <summary>
    /// Código postal de la dirección cargada.
    /// </summary>
    public string? CodigoPostal { get; set; }
    /// <summary>
    /// Mail de contacto del reclamante.
    /// </summary>
    public string? Mail { get; set; }
    /// <summary>
    /// Teléfono de contacto del reclamante.
    /// </summary>
    public string? Telefonos { get; set; }
    /// <summary>
    /// Id del tipo de técnico."
    /// </summary>
    public string? TecnicoTipoId { get; set; }
    /// <summary>
    /// Id del técnico.
    /// </summary>
    public string? TecnicoId { get; set; }
    /// <summary>
    /// Id del tipo de producto.
    /// </summary>
    public string? ProductoTipoId { get; set; }
    /// <summary>
    /// Id del producto.
    /// </summary>
    public string? ProductoId { get; set; }
    /// <summary>
    /// Usuario que creó el reclamo.
    /// </summary>
    public string? UserCreated { get; set; }
    /// <summary>
    /// Items asociados al reclamo.
    /// </summary>
    public ICollection<ItemReclamoWebZendeskDto>? ItemsReclamoWebZendesk { get; set; }
    public TrReclamoDto? ReclamoAsociado { get; set; }
    public string? Telefono2 { get; set; }
    public DateTime FechaCompra { get; set; }
    public string? Observaciones { get; set; }
}