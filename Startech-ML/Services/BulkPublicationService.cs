using System;
using System.Threading.Tasks;
using StartechML.Api;
using StartechML.Models;

namespace StartechML.Services
{
    // Servicio que maneja la lógica de creación de publicaciones
    public class BulkPublicationService
    {
        private readonly MercadoLibreClient _mlClient;

        public BulkPublicationService(MercadoLibreClient mlClient)
        {
            _mlClient = mlClient;
        }

        public async Task<string> PublishAsync(PublicationRequest publication)
        {
            // Validaciones básicas antes de llamar a la API
            if (string.IsNullOrWhiteSpace(publication.Title))
                throw new ArgumentException("El título es obligatorio");

            if (string.IsNullOrWhiteSpace(publication.CategoryId))
                throw new ArgumentException("La categoría es obligatoria");

            if (publication.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");

            if (publication.AvailableQuantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0");

            // Si todo está bien, delega la creación al cliente de ML
            return await _mlClient.CreatePublicationAsync(publication);
        }

        public async Task PublishAllAsync(List<PublicationRequest> publications)
        {
            foreach (var publication in publications)
            {
                try
                {
                    Console.WriteLine($"Publicando: {publication.Title}");

                    var result = await PublishAsync(publication);

                    Console.WriteLine("✔ Publicación creada con éxito");
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Error al publicar");
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("----------------------------------");
            }
        }

    }
}
