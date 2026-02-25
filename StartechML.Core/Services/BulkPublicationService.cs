using StartechML.Core.Api;
using StartechML.Core.Models;
using StartechML.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace StartechML.Core.Services
{
    // Servicio que maneja la lógica de creación de publicaciones
    public class BulkPublicationService
    {
        // Cliente que se comunica con la API de Mercado Libre
        private readonly MercadoLibreClient _mlClient;

        // Constructor: recibe el cliente ya configurado con access token
        public BulkPublicationService(MercadoLibreClient mlClient)
        {
            _mlClient = mlClient;
        }

        // PUBLICA UNA SOLA PUBLICACIÓN

        // Ahora devuelve Task<PublicationResponse>
        public async Task<PublicationResponse> PublishAsync(PublicationRequest publication)
        {
            Logger.Write($"Validando publicación: {publication.Title}", "Y", "Y", Logger.Mode.Info.ToString());

            // VALIDACIONES BÁSICAS

            if (string.IsNullOrWhiteSpace(publication.Title))
                throw new ArgumentException("El título es obligatorio");

            if (string.IsNullOrWhiteSpace(publication.CategoryId))
                throw new ArgumentException("La categoría es obligatoria");

            if (publication.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");

            if (publication.AvailableQuantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0");

            Logger.Write($"Enviando publicación a MercadoLibre: {publication.Title}", "Y", "Y", Logger.Mode.Info.ToString());

            // LLAMADA AL CLIENTE DE MERCADO LIBRE
    

            // El cliente devuelve un JSON en formato string
            var jsonResponse = await _mlClient.CreatePublicationAsync(publication);

            //  DESERIALIZAMOS EL JSON A UN OBJETO REAL

            var publicationResponse = JsonSerializer.Deserialize<PublicationResponse>(
                jsonResponse,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignora mayúsculas/minúsculas
                }
            );
            //Validamos que no sea null
            if (publicationResponse == null)
            {
                throw new Exception("Error al deserializar la respuesta de Mercado Libre.");
            }
            // Devolvemos el objeto real, NO un string
            return publicationResponse;
        }

        // PUBLICA MUCHAS PUBLICACIONES

        public async Task PublishAllAsync(List<PublicationRequest> publications)
        {
            foreach (var publication in publications)
            {
                try
                {
                    Logger.Write($"Publicando: {publication.Title}", "Y", "Y", Logger.Mode.Info.ToString());
                    Console.WriteLine($"Publicando: {publication.Title}");

                    // Ahora result es PublicationResponse, no string
                    var result = await PublishAsync(publication);

                    Console.WriteLine("✔ Publicación creada con éxito");
                    Console.WriteLine($"ID generado: {result.Id}");

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
