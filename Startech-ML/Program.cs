using StartechML.Api;
using StartechML.Models;
using StartechML.Services;
using StartechML.Utils;
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
                Console.WriteLine("StartechML - Crear publicaciones en Mercado Libre\n");
                //TOKEN OAUTH VÁLIDO
                string accessToken = "APP_USR-2804901742283043-021120-14066706e6fcbd181096f4d0a9ba1085-1170413717";

                Logger.Write("Token cargado correctamente", "Y", "Y", Logger.Mode.Info.ToString());
                //INICIALIZAR CLIENTE Y SERVICIO

                var mlClient = new MercadoLibreClient(accessToken);
                var publicationService = new BulkPublicationService(mlClient);
                //3️ CARGAR PUBLICACIONES DESDE JSON

                Console.WriteLine("Cargando publicaciones desde archivo JSON...\n");
                Logger.Write("Cargando publicaciones desde JSON", "Y", "Y", Logger.Mode.Info.ToString());

                var basePath = AppContext.BaseDirectory;
                var jsonPath = Path.Combine(basePath, "data", "Publication.json");

                List<PublicationRequest> publications = 
                    FilePublicationLoader.LoadFromJson(jsonPath);

                Logger.Write($"Se cargaron {publications.Count} publicaciones", "Y", "Y", Logger.Mode.Info.ToString());

                if (publications.Count == 0)
                {
                    Console.WriteLine("⚠️ No hay publicaciones para procesar.");
                    Logger.Write("No hay publicaciones para procesar", "Y", "Y", Logger.Mode.Info.ToString());
                    return;
                }

                //Ejecutar Publicacion masiva
                await publicationService.PublishAllAsync(publications);

                Logger.Write("Proceso finalizado correctamente", "Y", "Y", Logger.Mode.Info.ToString());
            }
            catch (Exception ex)
            {
                Logger.Write("Error general: " + ex.Message, "Y", "Y", Logger.Mode.Error.ToString());
            }

            Console.WriteLine("\nProceso finalizado");
            Console.ReadKey();
        }
    }
}

