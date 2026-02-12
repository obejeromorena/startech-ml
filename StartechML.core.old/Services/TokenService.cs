using System.Text.Json;
using StartechML.Web.Models;
using StartechML.Utils;

namespace StartechML.Web.Services
{
    public class TokenService
    {
        private readonly string _path = Path.Combine("Data", "token.json");
        private readonly MercadoAuthService _authService = new();

        public void SaveToken(TokenResponse token)
        {
            Directory.CreateDirectory("Data");

            var json = JsonSerializer.Serialize(token, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_path, json);

            Logger.Write("Token guardado correctamente.", "Y", "Y", "Info");
        }

        public TokenResponse? GetToken()
        {
            if (!File.Exists(_path))
                return null;

            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<TokenResponse>(json);
        }

        public async Task<string?> GetValidAccessTokenAsync()
        {
            var token = GetToken();

            if (token == null)
                return null;

            if (DateTime.Now < token.ExpirationDate)
                return token.AccessToken;

            Logger.Write("Token expirado. Renovando...", "Y", "Y", "Info");

            var newToken = await _authService.RefreshTokenAsync(token.RefreshToken);

            if (newToken == null)
                return null;

            SaveToken(newToken);

            return newToken.AccessToken;
        }
    }
}
