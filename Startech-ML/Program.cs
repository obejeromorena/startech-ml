using StartechML.Core.Models;
using StartechML.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StartechML
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Logger.SetLogPath(Path.Combine(AppContext.BaseDirectory, "Log"));
            Logger.SetLogLine(85);
            Logger.Write("Inicio aplicación StartechML (Consola)", "Y", "Y", "Info");

            try
            {
                var basePath = AppContext.BaseDirectory;
                var jsonPath = Path.Combine(basePath, "data", "Publication.json");

                // 🔹 Leer archivo JSON
                var json = await File.ReadAllTextAsync(jsonPath);

                // 🔹 Convertir a lista de PublicationRequest
                var publications = JsonSerializer.Deserialize<List<PublicationRequest>>(json);

                if (publications == null || publications.Count == 0)
                {
                    Logger.Write("No hay publicaciones para enviar.", "Y", "Y", "Info");
                    return;
                }

                using var httpClient = new HttpClient();

                foreach (var publication in publications)
                {
                    var content = new StringContent(
                        JsonSerializer.Serialize(publication),
                        Encoding.UTF8,
                        "application/json"
                    );

                    // 🔹 Enviar al Web
                    var response = await httpClient.PostAsync(
                        "https://localhost:7006/api/publications",
                        content
                    );

                    if (response.IsSuccessStatusCode)
                    {
                        Logger.Write("Publicación enviada al Web correctamente.", "Y", "Y", "Info");
                    }
                    else
                    {
                        Logger.Write("Error al enviar publicación al Web.", "Y", "Y", "Error");
                    }
                }

                Logger.Write("Proceso finalizado correctamente.", "Y", "Y", "Info");
            }
            catch (Exception ex)
            {
                Logger.Write("Error general en consola: " + ex.Message, "Y", "Y", "Error");
            }

            Console.WriteLine("\nProceso finalizado");
            Console.ReadKey();
        }
    }
}

