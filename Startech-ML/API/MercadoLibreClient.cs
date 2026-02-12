using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StartechML.Models;

namespace StartechML.Api
{
    // Cliente encargado de comunicarse con la API de Mercado Libre
    public class MercadoLibreClient
    {
        // Cliente HTTP reutilizable
        private readonly HttpClient _httpClient;

        // Constructor: recibe el access token
        public MercadoLibreClient(string accessToken)
        {
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
        }

        // Crea una publicación en Mercado Libre
        public async Task<string> CreatePublicationAsync(PublicationRequest publication)
        {
            // Convierte el objeto PublicationRequest a JSON
            var json = JsonSerializer.Serialize(publication);

            // Crea el contenido HTTP con encoding UTF-8 y tipo application/json
            var content = new StringContent(json, Encoding.UTF8, "application/json");

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
                throw new HttpRequestException(
                    $"Error al crear publicación: {response.StatusCode} - {responseBody}"
                );
            }

            // Devuelve la respuesta de Mercado Libre (JSON)
            return responseBody;
        }
    }
}

