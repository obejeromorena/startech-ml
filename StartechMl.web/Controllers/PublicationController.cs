using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StartechML.Core.Services;
using StartechML.Core.Api;
using StartechML.Core.Models;
using Startech_ML.Web.Storage;
using System.Text.Json;

namespace StartechML.web.Controllers
{
    [ApiController]
    [Route("api/publications")]
    public class PublicationController : ControllerBase
    {
        private readonly ILogger<PublicationController> _logger;

        //  Inyectamos el logger profesionalmente
        public PublicationController(ILogger<PublicationController> logger)
        {
            _logger = logger;
        }

        // =========================================
        // POST: Publica un producto en Mercado Libre
        // =========================================
        [HttpPost]
        public async Task<IActionResult> Publish([FromBody] PublicationRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando publicación en Mercado Libre...");

                // 1️ Obtenemos token válido
                var tokenService = new TokenService();
                var accessToken = await tokenService.GetValidAccessTokenAsync();

                if (string.IsNullOrEmpty(accessToken))
                {
                    _logger.LogWarning("No se encontró un token válido.");
                    return Unauthorized("No hay token válido.");
                }

                // 2️ Creamos cliente de Mercado Libre
                var mlClient = new MercadoLibreClient(accessToken);

                // 3️ Creamos servicio de publicación
                var service = new BulkPublicationService(mlClient);

                // 4️ Publicamos el producto
                var result = await service.PublishAsync(request);

                _logger.LogInformation("Publicación realizada correctamente.");

                // 5️ Guardamos la respuesta de la API en memoria
                // Esto es lo que luego el frontend va a mostrar
                var json = JsonSerializer.Serialize(result);
                PublicationStorage.Publications.Add(json);

                _logger.LogInformation("Publicación guardada en memoria.");

                // 6️ Devolvemos al cliente la respuesta de Mercado Libre
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al publicar en Mercado Libre.");
                return BadRequest(ex.Message);
            }
        }

        // =========================================
        // GET: Devuelve todas las publicaciones guardadas
        // =========================================
        [HttpGet]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Obteniendo publicaciones almacenadas.");

            return Ok(PublicationStorage.Publications);
        }
    }
}

