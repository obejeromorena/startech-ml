using Microsoft.AspNetCore.Mvc;
using System;
using StartechML.Core.Utils;

namespace StartechML.web.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            Logger.Write("Ingreso a /auth/login", "Y", "Y", Logger.Mode.Info.ToString());
            var clientId = "2804901742283043";

            var redirectUri = Uri.EscapeDataString(
                "https://antonio-loftless-winona.ngrok-free.dev/callback"
            );

            var url =
                "https://auth.mercadolibre.com.ar/authorization" +
                "?response_type=code" +
                "&client_id=" + clientId +
                "&redirect_uri=" + redirectUri;

            return Redirect(url);
        }
    }
}
