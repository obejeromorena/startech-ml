namespace StartechML.Core.Models
{
    // Modelo que representa la respuesta de Mercado Libre
    public class PublicationResponse
    {
        public string Id { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Status { get; set; } = string.Empty;

        public string Permalink { get; set; } = string.Empty;
    }
}