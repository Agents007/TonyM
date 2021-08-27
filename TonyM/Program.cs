using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TonyM.Models;
using TonyM.Modules;

namespace TonyM
{
    class Program
    {
        static async Task Main(string[] args)
        {
            #region Variable
            const string URL_INIT = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr&category=GPU&gpu=RTX%203090,RTX%203080%20Ti,RTX%203080,RTX%203070%20Ti,RTX%203070,RTX%203060%20Ti,RTX%203060&gpu_filter=RTX%203090~12,RTX%203080%20Ti~7,RTX%203080~16,RTX%203070%20Ti~3,RTX%203070~18,RTX%203060%20Ti~8,RTX%203060~2,RTX%202080%20SUPER~1,RTX%202080~0,RTX%202070%20SUPER~0,RTX%202070~0,RTX%202060~6,GTX%201660%20Ti~0,GTX%201660%20SUPER~9,GTX%201660~8,GTX%201650%20Ti~0,GTX%201650%20SUPER~3,GTX%201650~17";
            const int REFRESH = 1000;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            #endregion


            #region Initialisation
            DropFile.Delete();
            string jsonStr = await NvidiaApi.Connection(URL_INIT);
            InitCall gpuObj = JsonSerializer.Deserialize<InitCall>(jsonStr, options);

            var gpusAvailable = gpuObj.GetAllFe();

            Console.WriteLine("Salut c'est Tony. J'ai des contacts dans la Mafia.\n" +
                "\nQuelle carte graphique recherches tu ? (Entrer le numéro correspondant à la carte graphique souhaitée)");

            List<GraphicCard> gpusUser = UserSelection.GetSelection(gpusAvailable);

            Console.WriteLine("\nMerci, je consulte Laurent");
            #endregion


            #region Recherche RTX           
            while (true)
            {
                await Task.Delay(REFRESH);
                Console.Clear();

                string  gpusName = String.Join(", ", gpusUser.Select(g => g.ShortName).ToList());
                Console.WriteLine("== VOTRE SELECTION ==");            
                Console.WriteLine(gpusName + "\n");

                if (File.Exists(DropFile.PathAndFile))
                    DropFile.Display();

                gpusUser = gpusUser.Where(g => g.Wanted == true).ToList();
                Console.WriteLine("== VERIFICATION DES STOCKS ==");          

                IEnumerable<Task> tasks = gpusUser.Select(async gpuUser =>
                {
                    try
                    {
                        string jsonStr = await NvidiaApi.Connection(gpuUser.Link);
                        Call gpuObj = JsonSerializer.Deserialize<Call>(jsonStr, options);
                        bool drop = gpuObj.GetGpu();
                        if (drop)
                            gpuUser.Wanted = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur : " + ex.Message);
                    }
                });

                await Task.WhenAll(tasks);

                if (gpusUser.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Votre sélection est vide, un drop a déjà eu lieu pour les références choisies." +
                        "\nMerci de relancer l'application pour une nouvelle recherche");
                    break;
                }
            }
            #endregion
        }
    }
}
