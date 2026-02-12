using StartechML.Core.Api;
using StartechML.Core.Models;
using StartechML.Core.Utils;
using System;
using System.Threading.Tasks;

namespace StartechML.Core.Services
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
            Logger.Write($"Validando publicación: {publication.Title}", "Y", "Y", Logger.Mode.Info.ToString());

            // Validaciones básicas antes de llamar a la API
            if (string.IsNullOrWhiteSpace(publication.Title))
                throw new ArgumentException("El título es obligatorio");

            if (string.IsNullOrWhiteSpace(publication.CategoryId))
                throw new ArgumentException("La categoría es obligatoria");

            if (publication.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");

            if (publication.AvailableQuantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0");

            Logger.Write($"Enviando publicación a MercadoLibre: {publication.Title}", "Y", "Y", Logger.Mode.Info.ToString());

            // Si todo está bien, delega la creación al cliente de ML
            return await _mlClient.CreatePublicationAsync(publication);
        }

        public async Task PublishAllAsync(List<PublicationRequest> publications)
        {
            foreach (var publication in publications)
            {
                try
                {
                    Logger.Write($"Publicando: {publication.Title}", "Y", "Y", Logger.Mode.Info.ToString());
                    Console.WriteLine($"Publicando: {publication.Title}");

                    var result = await PublishAsync(publication);

                    Console.WriteLine("✔ Publicación creada con éxito");
                    Console.WriteLine(result);
                    Logger.Write($"Publicación creada correctamente: {publication.Title}", "Y", "Y", Logger.Mode.Info.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Error al publicar");
                    Console.WriteLine(ex.Message);
                    Logger.Write($"Error al publicar {publication.Title}: {ex.Message}", "Y", "Y", Logger.Mode.Error.ToString());
                }

                Console.WriteLine("----------------------------------");
            }
        }

    }
}
