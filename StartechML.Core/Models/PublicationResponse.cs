namespace StartechML.Core.Models
{
    // Modelo que representa la respuesta de Mercado Libre
    public class PublicationResponse
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }

        public string Permalink { get; set; }
    }
}