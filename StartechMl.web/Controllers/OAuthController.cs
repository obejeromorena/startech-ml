using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using StartechML.Utils;

namespace StartechML.web.Controllers
{
    [ApiController]
    [Route("callback")]
    public class OAuthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            Logger.Write("Callback OAuth invocado", "Y", "Y", Logger.Mode.Info.ToString());

            if (string.IsNullOrEmpty(code))
            {
                Logger.Write("Callback sin code", "Y", "Y", Logger.Mode.Error.ToString());
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

            Logger.Write("Token OAuth solicitado a MercadoLibre", "Y", "Y", Logger.Mode.Info.ToString());

            var json = await response.Content.ReadAsStringAsync();

            return Ok(json);
        }
    }
}
