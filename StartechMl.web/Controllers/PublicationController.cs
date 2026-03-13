using Microsoft.AspNetCore.Mvc;
using Startech_ML.Web.Storage;
using StartechML.Core.Api;
using StartechML.Core.Models;
using StartechML.Core.Services;
using System.Linq;
using StartechML.Core.Utils;
using System;

namespace StartechML.Web.Controllers
{
    [ApiController]
    [Route("api/publications")]
    public class PublicationController : ControllerBase
    {

        // POST: Publica un producto en Mercado Libre

        [HttpPost]
        public async Task<IActionResult> Publish([FromBody] PublicationRequest request)
        {
            try
            {
                Logger.Write("Iniciando publicación en Mercado Libre...", "Y", "Y", Logger.Mode.Info.ToString());


                // VALIDACIÓN PROFESIONAL DE IMÁGENES


                // Validar que la lista no sea null
                if (request.Pictures == null)
                {
                    Logger.Write("La lista de imágenes es null.", "Y", "Y", Logger.Mode.Info.ToString());
                    return BadRequest("Debe enviar al menos una imagen.");
                }

                // Validar que tenga al menos 1 imagen
                if (request.Pictures.Count == 0)
                {
                    Logger.Write("La publicación no contiene imágenes.", "Y", "Y", Logger.Mode.Info.ToString());
                    return BadRequest("Debe incluir al menos una imagen.");
                }

                // Validar máximo permitido por Mercado Libre
                if (request.Pictures.Count > 12)
                {
                    Logger.Write("La publicación supera el límite de 12 imágenes.", "Y", "Y", Logger.Mode.Info.ToString());
                    return BadRequest("No se pueden enviar más de 12 imágenes.");
                }

                // Validar que ninguna imagen tenga URL vacía
                if (request.Pictures.Any(p => string.IsNullOrWhiteSpace(p.Source)))
                {
                    Logger.Write("Una o más imágenes tienen URL vacía.", "Y", "Y", Logger.Mode.Info.ToString());

                    return BadRequest("Todas las imágenes deben tener una URL válida.");
                }

                // FIN VALIDACIÓN

                // Obtener token válido
                var tokenService = new TokenService();
                var accessToken = await tokenService.GetValidAccessTokenAsync();

                if (string.IsNullOrEmpty(accessToken))
                {
                    Logger.Write("No se encontró un token válido.", "Y", "Y", Logger.Mode.Info.ToString());
                    return Unauthorized("No hay token válido.");
                }

                // Crear cliente de Mercado Libre
                var mlClient = new MercadoLibreClient(accessToken);

                // Crear servicio de publicación
                var service = new BulkPublicationService(mlClient);

                // Publicar
                var result = await service.PublishAsync(request);

                Logger.Write("Publicación realizada correctamente.", "Y", "Y", Logger.Mode.Info.ToString());

                // Guardar en memoria
                PublicationStorage.Publications.Add(result);

                Logger.Write("Publicación guardada en memoria.", "Y", "Y", Logger.Mode.Info.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Write($"Error al publicar en Mercado Libre: {ex.Message}", "Y", "Y", Logger.Mode.Error.ToString());
                return BadRequest(ex.Message);
            }
        }

        // GET: Devuelve todas las publicaciones guardadas

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                Logger.Write("Obteniendo publicaciones almacenadas.", "Y", "Y", Logger.Mode.Info.ToString());
                return Ok(PublicationStorage.Publications);
            }
            catch (Exception ex)
            {
                Logger.Write("Error al obtener publicaciones: {ex.Message}", "Y", "Y", Logger.Mode.Info.ToString());
                return BadRequest("Error al obtener publicaciones.");
            }
        }

        // GET: Obtiene publicaciones reales desde Mercado Libre

        [HttpGet("ml")]
        public async Task<IActionResult> GetFromML()
        {
            try
            {
                Logger.Write("Consultando publicaciones en ML", "Y", "Y", Logger.Mode.Info.ToString());

                var tokenService = new TokenService();
                var accessToken = await tokenService.GetValidAccessTokenAsync();

                if (string.IsNullOrEmpty(accessToken))
                {
                    return Unauthorized("No hay token válido.");
                }

                var mlClient = new MercadoLibreClient(accessToken);

                // 1️⃣ Obtener usuario del token
                var userJson = await mlClient.GetMyUserAsync();
                var userDoc = System.Text.Json.JsonDocument.Parse(userJson);
                var userId = userDoc.RootElement.GetProperty("id").GetInt64();

                Logger.Write($"Usuario autenticado: {userId}", "Y", "Y", Logger.Mode.Info.ToString());

                // 2️⃣ Obtener IDs de publicaciones
                var result = await mlClient.GetPublicationsAsync(userId.ToString());

                var doc = System.Text.Json.JsonDocument.Parse(result);
                var ids = doc.RootElement.GetProperty("results");

                var publications = new List<PublicationResponse>();

                // 3️⃣ Obtener datos completos de cada publicación
                foreach (var id in ids.EnumerateArray())
                {
                    var itemId = id.GetString();

                    var itemJson = await mlClient.GetItemAsync(itemId!);
                    var itemDoc = System.Text.Json.JsonDocument.Parse(itemJson);

                    var root = itemDoc.RootElement;

                    publications.Add(new PublicationResponse
                    {
                        Id = root.GetProperty("id").GetString() ?? string.Empty,
                        Title = root.GetProperty("title").GetString() ?? string.Empty,
                        Price = (decimal)root.GetProperty("price").GetDouble(),
                        Status = root.GetProperty("status").GetString() ?? string.Empty,
                        Permalink = root.GetProperty("permalink").GetString() ?? string.Empty
                    });
                }

                return Ok(publications);
            }
            catch (Exception ex)
            {
                Logger.Write($"Error al obtener publicaciones: {ex.Message}", "Y", "Y", Logger.Mode.Error.ToString());
                return BadRequest("Error al consultar publicaciones.");
            }
        }

        // PUT: Modifica el precio de una publicación en ML

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePriceRequest request)
        {
            try
            {
                Logger.Write($"Actualizando publicación {id}", "Y", "Y", Logger.Mode.Info.ToString());

                var tokenService = new TokenService();
                var accessToken = await tokenService.GetValidAccessTokenAsync();

                if (string.IsNullOrEmpty(accessToken))
                {
                    Logger.Write("No se encontró token válido.", "Y", "Y", Logger.Mode.Info.ToString());
                    return Unauthorized("No hay token válido.");
                }

                var mlClient = new MercadoLibreClient(accessToken);

                var result = await mlClient.UpdatePublicationAsync(id, new
                {
                    price = request.Price
                });

                Logger.Write($"Publicación {id} actualizada correctamente.", "Y", "Y", Logger.Mode.Info.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Write($"Error al actualizar publicación: {ex.Message}", "Y", "Y", Logger.Mode.Error.ToString());
                return BadRequest("Error al actualizar publicación.");
            }
        }

        // DELETE: Cierra una publicación en ML

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                Logger.Write($"Cerrando publicación {id}", "Y", "Y", Logger.Mode.Info.ToString());

                var tokenService = new TokenService();
                var accessToken = await tokenService.GetValidAccessTokenAsync();

                if (string.IsNullOrEmpty(accessToken))
                {
                    Logger.Write("No se encontró token válido.", "Y", "Y", Logger.Mode.Info.ToString());
                    return Unauthorized("No hay token válido.");
                }

                var mlClient = new MercadoLibreClient(accessToken);

                var result = await mlClient.ClosePublicationAsync(id);

                Logger.Write($"Publicación {id} cerrada correctamente.", "Y", "Y", Logger.Mode.Info.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Write($"Error al cerrar publicación: {ex.Message}", "Y", "Y", Logger.Mode.Error.ToString());
                return BadRequest("Error al cerrar publicación.");
            }
        }
    }
}
