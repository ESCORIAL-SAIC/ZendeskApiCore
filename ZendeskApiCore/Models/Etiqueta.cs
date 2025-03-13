namespace ZendeskApiCore.Models
{
    public class Etiqueta
    {
        public int Numero { get; set; }
        public Guid ProductoId { get; set; }
        public string ProductoCodigo { get; set; }
        public string ProductoNombre { get; set; }
        public string ProductoTipo { get; set; }
    }
}
