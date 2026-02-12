using Microsoft.AspNetCore.Mvc;
using StartechML.Core.Utils;
using StartechML.Core.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StartechML.web.Controllers
{
    [ApiController]
    [Route("callback")]
    public class OAuthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest("No se recibió el code");
                }

                var authService = new MercadoAuthService();
                var tokenService = new TokenService();

                var token = await authService.GetTokenAsync(code);

                tokenService.SaveToken(token);

                return Ok("Token guardado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error real: " + ex.Message);
            }
        }
    }
}
