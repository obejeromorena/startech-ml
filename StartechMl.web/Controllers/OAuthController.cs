using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StartechML.web.Controllers
{
    [ApiController]
    [Route("callback")]
    public class OAuthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("No se recibió el code");
            }

            var clientId = "2804901742283043";
            var clientSecret = "pnEwcBqgRX08NdfnzdmzhJedSmXY1cRm";
            var redirectUri = "https://antonio-loftless-winona.ngrok-free.dev/callback";

            using var http = new HttpClient();

            var data = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", code },
                { "redirect_uri", redirectUri }
            };

            var response = await http.PostAsync(
                "https://api.mercadolibre.com/oauth/token",
                new FormUrlEncodedContent(data)
            );

            var json = await response.Content.ReadAsStringAsync();

            return Ok(json);
        }
    }
}
