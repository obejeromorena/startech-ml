using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using StartechML.Models;

namespace StartechML.Services
{
    // Clase estática para cargar publicaciones desde un archivo JSON
    public static class FilePublicationLoader
    {
        // Carga una lista de publicaciones desde un archivo JSON
        public static List<PublicationRequest> LoadFromJson(string path)
        {
            // Verifica si el archivo existe
            if (!File.Exists(path))
                throw new FileNotFoundException($"No se encontró el archivo: {path}");

            // Lee todo el contenido del archivo
            var json = File.ReadAllText(path);

            // Opciones del serializador
            var options = new JsonSerializerOptions
            {
                // Permite que las mayúsculas/minúsculas no importen
                PropertyNameCaseInsensitive = true
            };

            // Convierte el JSON en una lista de PublicationRequest
            return JsonSerializer.Deserialize<List<PublicationRequest>>(json, options)
                   ?? new List<PublicationRequest>();
        }
    }
}
