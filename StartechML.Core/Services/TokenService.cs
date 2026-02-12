using StartechML.Core.Models;
using StartechML.Core.Utils;
using System.Text.Json;
using System;
using System.IO;
using System.Threading.Tasks;


namespace StartechML.Core.Services
{
    public class TokenService
    {
        private readonly string _path;
        private readonly MercadoAuthService _authService = new();

        public TokenService()
        {
            // Esto apunta a:
            // bin\Debug\net10.0\
            var basePath = AppContext.BaseDirectory;

            // Subimos 3 niveles hasta la raíz del proyecto
            var projectRoot = Directory.GetParent(basePath)!.Parent!.Parent!.FullName;

            // Creamos ruta SharedData/token.json
            _path = Path.Combine(projectRoot, "SharedData", "token.json");
        }

        public void SaveToken(TokenResponse token)
        {
            var directory = Path.GetDirectoryName(_path);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory!);

            var json = JsonSerializer.Serialize(token, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_path, json);

            Logger.Write("Token guardado correctamente en SharedData.", "Y", "Y", "Info");
        }

        public TokenResponse? GetToken()
        {
            if (!File.Exists(_path))
            {
                Logger.Write("No existe token.json en SharedData.", "Y", "Y", "Info");
                return null;
            }

            var json = File.ReadAllText(_path);

            return JsonSerializer.Deserialize<TokenResponse>(json);
        }

        public async Task<string?> GetValidAccessTokenAsync()
        {
            var token = GetToken();

            if (token == null)
            {
                Logger.Write("No hay token guardado.", "Y", "Y", "Error");
                return null;
            }

            if (DateTime.Now < token.ExpirationDate)
            {
                Logger.Write("Token válido encontrado.", "Y", "Y", "Info");
                return token.AccessToken;
            }

            Logger.Write("Token expirado. Intentando renovar...", "Y", "Y", "Info");

            if (string.IsNullOrEmpty(token.RefreshToken))
            {
                Logger.Write("No hay refresh token disponible.", "Y", "Y", "Error");
                return null;
            }

            var newToken = await _authService.RefreshTokenAsync(token.RefreshToken);

            if (newToken == null)
            {
                Logger.Write("No se pudo renovar el token.", "Y", "Y", "Error");
                return null;
            }

            SaveToken(newToken);

            return newToken.AccessToken;
        }
    }
}

