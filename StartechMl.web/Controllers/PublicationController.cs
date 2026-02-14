using Microsoft.AspNetCore.Mvc;
using Startech_ML.Web.Storage;
using StartechML.Core.Api;
using StartechML.Core.Models;
using StartechML.Core.Services;

namespace StartechML.web.Controllers
{
    [ApiController]
    [Route("api/publications")]
    public class PublicationController : ControllerBase
    {
        private readonly ILogger<PublicationController> _logger;

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

                var tokenService = new TokenService();
                var accessToken = await tokenService.GetValidAccessTokenAsync();

                if (string.IsNullOrEmpty(accessToken))
                {
                    _logger.LogWarning("No se encontró un token válido.");
                    return Unauthorized("No hay token válido.");
                }

                var mlClient = new MercadoLibreClient(accessToken);
                var service = new BulkPublicationService(mlClient);

                var result = await service.PublishAsync(request);

                _logger.LogInformation("Publicación realizada correctamente.");

                //  CAMBIO IMPORTANTE
                // Guardamos el objeto directamente (NO string)
                PublicationStorage.Publications.Add(result);

                _logger.LogInformation("Publicación guardada en memoria.");

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

