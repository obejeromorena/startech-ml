using System.Net.Http;
using System.Text.Json;
using StartechML.Core.Models;

namespace StartechML.Core.Services
{
    public class MercadoAuthService
    {
        private readonly HttpClient _http = new HttpClient();

        private const string ClientId = "2804901742283043";
        private const string ClientSecret = "pnEwcBqgRX08NdfnzdmzhJedSmXY1cRm";
        private const string RedirectUri = "https://antonio-loftless-winona.ngrok-free.dev/callback";

        public async Task<TokenResponse?> GetTokenAsync(string code)
        {
            var data = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "code", code },
                { "redirect_uri", RedirectUri }
            };

            var response = await _http.PostAsync(
                "https://api.mercadolibre.com/oauth/token",
                new FormUrlEncodedContent(data)
            );

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error ML: {response.StatusCode} - {errorContent}");
            }



            var token = JsonSerializer.Deserialize<TokenResponse>(json);

            if (token != null)
                token.ExpirationDate = DateTime.Now.AddSeconds(token.ExpiresIn);

            return token;
        }

        public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
        {
            var data = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "refresh_token", refreshToken }
            };

            var response = await _http.PostAsync(
                "https://api.mercadolibre.com/oauth/token",
                new FormUrlEncodedContent(data)
            );

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return null;

            var token = JsonSerializer.Deserialize<TokenResponse>(json);

            if (token != null)
                token.ExpirationDate = DateTime.Now.AddSeconds(token.ExpiresIn);

            return token;
        }
    }
}

