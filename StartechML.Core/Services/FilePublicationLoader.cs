using StartechML.Core.Models;
using StartechML.Core.Utils;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace StartechML.Core.Services
{
    // Clase estática para cargar publicaciones desde un archivo JSON
    public static class FilePublicationLoader
    {
        // Carga una lista de publicaciones desde un archivo JSON
        public static List<PublicationRequest> LoadFromJson(string path)
        {
            Logger.Write($"Intentando cargar archivo JSON: {path}", "Y", "Y", Logger.Mode.Info.ToString());

            // Verifica si el archivo existe
            if (!File.Exists(path)) 
            {
                Logger.Write($"Archivo no encontrado: {path}", "Y", "Y", Logger.Mode.Error.ToString());
                throw new FileNotFoundException($"No se encontró el archivo: {path}");
            }


            // Lee todo el contenido del archivo
            var json = File.ReadAllText(path);

            // Opciones del serializador
            var options = new JsonSerializerOptions
            {
                // Permite que las mayúsculas/minúsculas no importen
                PropertyNameCaseInsensitive = true
            };

            // Convierte el JSON en una lista de PublicationRequest
            var publications = JsonSerializer.Deserialize<List<PublicationRequest>>(json, options)
            ?? new List<PublicationRequest>();


            Logger.Write($"Se cargaron {publications.Count} publicaciones desde JSON",
                "Y",
                "Y",
                Logger.Mode.Info.ToString());

            return publications;
        }
    }
}
