using System;
using System.Threading.Tasks;
using StartechML.Api;
using StartechML.Models;
using StartechML.Services;
using System.Collections.Generic;
using System.IO;


namespace StartechML
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("StartechML - Crear publicaciones en Mercado Libre\n");


            //TOKEN OAUTH VÁLIDO
            string accessToken = "APP_USR-2804901742283043-021120-14066706e6fcbd181096f4d0a9ba1085-1170413717";


            //INICIALIZAR CLIENTE Y SERVICIO

            var mlClient = new MercadoLibreClient(accessToken);
            var publicationService = new BulkPublicationService(mlClient);


            //3️ CARGAR PUBLICACIONES DESDE JSON

            Console.WriteLine("Cargando publicaciones desde archivo JSON...\n");

            List<PublicationRequest> publications;

            try
            {
                var basePath = AppContext.BaseDirectory;
                var jsonPath = Path.Combine(basePath, "data", "Publication.json");

                publications = FilePublicationLoader.LoadFromJson(jsonPath);

            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error al cargar el archivo de publicaciones:");
                Console.WriteLine(ex.Message);
                return;
            }

            if (publications.Count == 0)
            {
                Console.WriteLine("⚠️ No hay publicaciones para procesar.");
                return;
            }
 
            //4️ EJECUTAR PUBLICACIÓN MASIVA
            try
            {
                await publicationService.PublishAllAsync(publications);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error general en el proceso:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nProceso finalizado");
            Console.ReadKey();
        }
    }
}

