using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StartechML.Core.Models;
using StartechML.Core.Utils;


namespace StartechML.Core.Api
{
    // Cliente encargado de comunicarse con la API de Mercado Libre
    public class MercadoLibreClient
    {
        // Cliente HTTP reutilizable
        private readonly HttpClient _httpClient;

        // Constructor: recibe el access token
        public MercadoLibreClient(string accessToken)
        {
            Logger.Write("Inicializando MercadoLibreClient", "Y", "Y", Logger.Mode.Info.ToString());

            // Handler HTTP (configuración avanzada)
            var handler = new HttpClientHandler
            {
                UseProxy = false
            };

            // Inicializa HttpClient
            _httpClient = new HttpClient(handler);

            // Header Authorization: Bearer TOKEN
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            // Indica que esperamos JSON como respuesta
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
            Logger.Write("MercadoLibreClient inicializado correctamente", "Y", "Y", Logger.Mode.Info.ToString());
        }
        //da el token
        public async Task<string> GetMyUserAsync()
        {
            var response = await _httpClient.GetAsync("https://api.mercadolibre.com/users/me");

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Logger.Write($"Error obteniendo usuario: {body}", "Y", "Y", Logger.Mode.Error.ToString());
                throw new HttpRequestException(body);
            }

            return body;
        }
        // Crea una publicación en Mercado Libre
        public async Task<string> CreatePublicationAsync(PublicationRequest publication)
        {
            Logger.Write($"Serializando publicación: {publication.Title}", "Y", "Y", Logger.Mode.Info.ToString());
            // Convierte el objeto PublicationRequest a JSON
            var json = JsonSerializer.Serialize(publication);

            // Crea el contenido HTTP con encoding UTF-8 y tipo application/json
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Logger.Write("Enviando POST a https://api.mercadolibre.com/items", "Y", "Y", Logger.Mode.Info.ToString());

            // Hace el POST al endpoint oficial de creación de ítems
            var response = await _httpClient.PostAsync(
                "https://api.mercadolibre.com/items",
                content
            );

            // Lee la respuesta como texto
            var responseBody = await response.Content.ReadAsStringAsync();

            // Si la API devuelve error, se lanza una excepción
            if (!response.IsSuccessStatusCode)
            {
                Logger.Write(
                    $"Error API ML: {response.StatusCode} - {responseBody}",
                    "Y",
                    "Y",
                    Logger.Mode.Error.ToString()
                );
                throw new HttpRequestException(
                    $"Error al crear publicación: {response.StatusCode} - {responseBody}"
                );
            }

            Logger.Write(
                $"Publicación creada exitosamente: {publication.Title}",
                "Y",
                "Y",
                Logger.Mode.Info.ToString()
            );

            // Devuelve la respuesta de Mercado Libre (JSON)
            return responseBody;
        }

        // Obtiene las publicaciones del usuario en Mercado Libre
        public async Task<string> GetPublicationsAsync(string userId)
        {
            Logger.Write($"Obteniendo publicaciones del usuario {userId}", "Y", "Y", Logger.Mode.Info.ToString());

            var response = await _httpClient.GetAsync(
                $"https://api.mercadolibre.com/users/{userId}/items/search"
            );

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Logger.Write(
                    $"Error al obtener publicaciones: {response.StatusCode} - {responseBody}",
                    "Y",
                    "Y",
                    Logger.Mode.Error.ToString()
                );

                throw new HttpRequestException(
                    $"Error al obtener publicaciones: {response.StatusCode} - {responseBody}"
                );
            }

            Logger.Write("Publicaciones obtenidas correctamente", "Y", "Y", Logger.Mode.Info.ToString());

            return responseBody;
        }

        // Modifica una publicación existente en Mercado Libre
        public async Task<string> UpdatePublicationAsync(string itemId, object updateData)
        {
            Logger.Write($"Actualizando publicación {itemId}", "Y", "Y", Logger.Mode.Info.ToString());

            var json = JsonSerializer.Serialize(updateData);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(
                $"https://api.mercadolibre.com/items/{itemId}",
                content
            );

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Logger.Write(
                    $"Error al actualizar publicación: {response.StatusCode} - {responseBody}",
                    "Y",
                    "Y",
                    Logger.Mode.Error.ToString()
                );

                throw new HttpRequestException(
                    $"Error al actualizar publicación: {response.StatusCode} - {responseBody}"
                );
            }

            Logger.Write($"Publicación {itemId} actualizada correctamente", "Y", "Y", Logger.Mode.Info.ToString());

            return responseBody;
        }

        // Cierra (elimina) una publicación en Mercado Libre
        public async Task<string> ClosePublicationAsync(string itemId)
        {
            Logger.Write($"Cerrando publicación {itemId}", "Y", "Y", Logger.Mode.Info.ToString());

            var body = new
            {
                status = "closed"
            };

            var json = JsonSerializer.Serialize(body);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(
                $"https://api.mercadolibre.com/items/{itemId}",
                content
            );

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Logger.Write(
                    $"Error al cerrar publicación: {response.StatusCode} - {responseBody}",
                    "Y",
                    "Y",
                    Logger.Mode.Error.ToString()
                );

                throw new HttpRequestException(
                    $"Error al cerrar publicación: {response.StatusCode} - {responseBody}"
                );
            }

            Logger.Write($"Publicación {itemId} cerrada correctamente", "Y", "Y", Logger.Mode.Info.ToString());

            return responseBody;
        }

        public async Task<string> GetItemAsync(string itemId)
        {
            var response = await _httpClient.GetAsync($"https://api.mercadolibre.com/items/{itemId}");

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Logger.Write($"Error obteniendo item {itemId}: {body}", "Y", "Y", Logger.Mode.Error.ToString());
                throw new HttpRequestException(body);
            }

            return body;
        }
    }
}

