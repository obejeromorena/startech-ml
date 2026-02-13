using Microsoft.AspNetCore.Mvc;
using StartechML.Core.Services;
using StartechML.Core.Api;
using StartechML.Core.Models;

namespace StartechML.web.Controllers
{
    [ApiController]
    [Route("api/publications")]
    public class PublicationController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Publish([FromBody] PublicationRequest request)
        {
            try
            {
                var tokenService = new TokenService();
                var accessToken = await tokenService.GetValidAccessTokenAsync();

                if (string.IsNullOrEmpty(accessToken))
                    return Unauthorized("No hay token válido.");

                var mlClient = new MercadoLibreClient(accessToken);
                var service = new BulkPublicationService(mlClient);

                var result = await service.PublishAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

