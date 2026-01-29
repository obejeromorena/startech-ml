using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace StartechML
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("StartechML - Integración con Mercado Libre\n");

         
            string accessToken = "APP_USR-2804901742283043-012819-d30959df714bd365b2a0b831548ad02d-1170413717";

            using var client = new HttpClient();

            
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);


            var response = await client.GetAsync(
                "https://api.mercadolibre.com/users/1170413717/items/search"
            );


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                return;
            }

            
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Respuesta de Mercado Libre:");
            Console.WriteLine(json);

            Console.WriteLine("\nPresione una tecla para salir...");
            Console.ReadKey();
        }
    }
}
