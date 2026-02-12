using StartechML.Core.Api;
using StartechML.Core.Services;
using StartechML.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace StartechML
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //1️ INICIALIZAR LOGGER
            Logger.SetLogPath(Path.Combine(AppContext.BaseDirectory, "Log"));
            Logger.SetLogLine(85);
            Logger.Write("Inicio aplicación StartechML (Consola)", "Y", "Y", Logger.Mode.Info.ToString());



            try
            {
                var tokenService = new TokenService();
                var accessToken = await tokenService.GetValidAccessTokenAsync();

                if (string.IsNullOrEmpty(accessToken))
                {
                    Logger.Write("No se pudo obtener un token válido.", "Y", "Y", "Error");
                    Console.WriteLine("No hay token válido. Primero autenticarse desde la WEB.");
                    return;
                }

                var mlClient = new MercadoLibreClient(accessToken);
                var publicationService = new BulkPublicationService(mlClient);

                var basePath = AppContext.BaseDirectory;
                var jsonPath = Path.Combine(basePath, "data", "Publication.json");

                var publications = FilePublicationLoader.LoadFromJson(jsonPath);

                if (publications.Count == 0)
                {
                    Logger.Write("No hay publicaciones para procesar.", "Y", "Y", "Info");
                    return;
                }

                await publicationService.PublishAllAsync(publications);

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

