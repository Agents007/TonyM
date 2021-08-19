using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
            // ---------------Variable-------------------------------------------------------------------------
            const string URL_INIT = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr&category=GPU&gpu=RTX%203090,RTX%203080%20Ti,RTX%203080,RTX%203070%20Ti,RTX%203070,RTX%203060%20Ti,RTX%203060&gpu_filter=RTX%203090~12,RTX%203080%20Ti~7,RTX%203080~16,RTX%203070%20Ti~3,RTX%203070~18,RTX%203060%20Ti~8,RTX%203060~2,RTX%202080%20SUPER~1,RTX%202080~0,RTX%202070%20SUPER~0,RTX%202070~0,RTX%202060~6,GTX%201660%20Ti~0,GTX%201660%20SUPER~9,GTX%201660~8,GTX%201650%20Ti~0,GTX%201650%20SUPER~3,GTX%201650~17";
            const string URL_BASE = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr&category=GPU&gpu=";
            const int REFRESH = 1000;

            string path = "Drop";
            string filename = "drops.txt";
            string dropFile = Path.Combine(path, filename);


            // ---------------Premier lancement de l'application-------------------------------------------------
            GlobalMethod.CreateDropFile(path);
            var client = new NvidiaApi(URL_INIT);
            using var jsonParse = await client.Connection();
            List<GraphicsCard> gpusAvailable = CustomDeserialize.DeserializeJson(jsonParse);

            Console.WriteLine("Salut c'est Tony. J'ai des contacts dans la Mafia.\n" +
                "\nQuelle carte graphique recherches tu ? (Entrer le numéro correspondant à la carte graphique souhaitée)");

            List<GraphicsCard> gpusUser = UserSelection.getUserSelection(gpusAvailable);

            Console.WriteLine();
            Console.WriteLine("Merci, je consulte Laurent");
            // ----------------------Fin Initialisation--------------------------------------------



            // ---------------Recherche des RTX FE-------------------------------------------------       
            while (true)
            {
                await Task.Delay(REFRESH);
                Console.Clear();

                Console.WriteLine("-- VOTRE SELECTION --");
                string shortName = String.Join(",", gpusUser.Select(g => g.DisplayName).ToList());
                Console.WriteLine(shortName.Replace("NVIDIA RTX ", ""));

                if (File.Exists(dropFile))
                    GlobalMethod.DisplayOldDrop(dropFile);

                Console.WriteLine();
                Console.WriteLine("-- VERIFICATION DES STOCKS --");

                gpusUser = gpusUser.Where(g => g.Wanted == true).ToList();

                IEnumerable<Task> tasks = gpusUser.Select(async gpuUser =>
                {
                    var client = new NvidiaApi(URL_BASE + gpuUser.NameForUrl());
                    using var jsonParse = await client.Connection();
                    GraphicsCard gpu = CustomDeserialize.DeserializeJson(jsonParse).First();
                    bool drop = gpu.SearchStock(dropFile);
                    if (drop)
                        gpuUser.Wanted = false;
                });
                await Task.WhenAll(tasks);

                if (gpusUser.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Votre sélection est vide, un drop a déjà eu lieu pour les références choisies.\nMerci de relancer l'application pour une nouvelle recherche");
                    break;
                }
            }
        }
    }
}
