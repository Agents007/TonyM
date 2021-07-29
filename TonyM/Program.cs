using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace TonyM
{
    public static partial class JsonExtensions
    {
        public static T ToObject<T>(this JsonElement element, JsonSerializerOptions options = null)
        {
            var bufferWriter = new System.Buffers.ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
                element.WriteTo(writer);
            return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
        }

        public static T ToObject<T>(this JsonDocument document, JsonSerializerOptions options = null)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            return document.RootElement.ToObject<T>(options);
        }
    }



    class Program
    {
        static string CreateDropFile()
        {
            var path = "Drop";
            string filename = "drops.txt";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string pathAndFile = Path.Combine(path, filename);

            if (File.Exists(pathAndFile))
            {
                File.Delete(pathAndFile);
            }

            return pathAndFile;
        }
        static void WriteDrop(string pathAndFile, string name, string link)
        {
            DateTime date = DateTime.Now;
            CultureInfo cultureFrancais = CultureInfo.GetCultureInfo("fr-FR");

            string dateStr = date.ToString("dd/MM HH:mm:ss", cultureFrancais);
            string drop = dateStr + " : " + name.Replace("NVIDIA RTX ", "") + " -> " + link + "\n";
            File.AppendAllText(pathAndFile, drop);
        }

        static void DisplayGpuWanted(List<string> gpuWanted)
        {
            Console.Write("-- VOTRE SELECTION --\n");
            Console.WriteLine(String.Join(", ", gpuWanted.OrderBy(g => g)).Replace("NVIDIA RTX ", ""));
            Console.WriteLine();
            Console.WriteLine("-- VERIFICATION DES STOCKS --");
        }

        static void DisplayOldDrop(string pathAndFile)
        {
            Console.WriteLine();
            Console.WriteLine("-- HISTORIQUE DES DROPS --");
            string oldDrop = File.ReadAllText(pathAndFile);
            Console.WriteLine(oldDrop);
        }

        static void OpenBuyPage(string link)
        {
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = link;
            System.Diagnostics.Process.Start(psi);
        }

        static void SoundAlert()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.Beep();
            }
        }



        // Constitution de la liste des GPU Visible via l'API, et la liste de sélection utilisateur
        static List<string> GetGpuWanted(List<GraphicsCard> gpusObj)
        {
            List<string> gpusAvailable = new List<string>();
            foreach (var gpuObj in gpusObj)
            {
                gpusAvailable.Add(gpuObj.displayName);
            }
            gpusAvailable = gpusAvailable
                .OrderByDescending(g => g)
                .ToList();


            List<string> gpusUserSelect = new List<string>();


            for (int i = 0; i < gpusAvailable.Count; i++)
            {
                Console.WriteLine("Choix " + (i + 1) + " : " + gpusAvailable[i]);
            }
            Console.WriteLine("Choix 10 : SELECTION TERMINEE\n");


            while (true)
            {
                Console.Write("Votre choix : ");
                string choice = Console.ReadLine();
                try
                {
                    int choiceInt = int.Parse(choice);
                    if ((choiceInt > 0) && (choiceInt < (gpusAvailable.Count + 1)))
                    {
                        if (gpusUserSelect.Contains(gpusAvailable[choiceInt - 1]))
                        {
                            Console.WriteLine("Ce GPU fait déjà parti de votre sélection\n");
                        }
                        else
                        {
                            gpusUserSelect.Add(gpusAvailable[choiceInt - 1].Replace("NVIDIA ", ""));
                            Console.WriteLine(gpusAvailable[choiceInt - 1] + " ajoutée à la liste. Ajouter une autre carte ou terminer la sélection.\n");
                        }
                    }
                    else if ((choiceInt == 10) && (gpusUserSelect.Count > 0))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Votre choix n'est pas valide\n");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Erreur : Vous devez entrer un nombre\n");
                }
            }
            return gpusUserSelect;
        }


        // Récupération API
        static JsonDocument ConnectionApi(string url)
        {
            var webClient = new WebClient();
            webClient.Headers.Add("Accept", "application/json");
            webClient.Headers.Add("pragma", "no-cache");
            webClient.Headers.Add("cache-control", "no-cache");
            webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 OPR/77.0.4054.277");
            string json = null;
            try
            {
                json = webClient.DownloadString(url);
                var jsonParse = JsonDocument.Parse(json);
                return jsonParse;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return null;
            }
        }


        //  Deserialisation OBJ
        static List<GraphicsCard> GenerateGpu(JsonDocument jsonParse)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            List<GraphicsCard> searchedProducts = jsonParse.RootElement
                .GetProperty("searchedProducts")
                .GetProperty("productDetails")
                .EnumerateArray()
                .Where(n => n.GetProperty("isFounderEdition").GetBoolean())
                .Select(n => n.ToObject<GraphicsCard>(options))
                .ToList();

            GraphicsCard featuredProduct = jsonParse.RootElement
                .GetProperty("searchedProducts")
                .GetProperty("featuredProduct")
                .ToObject<GraphicsCard>(options);


            if ((featuredProduct != null) && (searchedProducts != null))
            {
                searchedProducts.Add(featuredProduct);
                return searchedProducts;
            }
            else if ((featuredProduct != null) && (searchedProducts == null))
            {
                List<GraphicsCard> featuredProductList = new List<GraphicsCard>();
                featuredProductList.Add(featuredProduct);
                return featuredProductList;
            }
            else if ((featuredProduct == null) && (searchedProducts != null))
            {
                return searchedProducts;
            } 
            return null;
        }


        

        // Check si le gpu est en stock
        static bool SearchGpu(List<GraphicsCard> gpus, string pathAndFile)
        {
            if (gpus.Count > 1)
            {
                Console.WriteLine("Problème avec l'API");
            }

            GraphicsCard gpu = gpus.First();

                string link = gpu.retailers.Select(g => g.purchaseLink).ToList().First();

                if ((gpu.prdStatus != "out_of_stock") && (!String.IsNullOrEmpty(link)))
                {
                    OpenBuyPage(link);
                    SoundAlert();
                    WriteDrop(pathAndFile, gpu.displayName, link);
                    return true;
                }
                else
                {
                    Console.WriteLine(gpu.displayName + " : " + gpu.prdStatus);
                }     

            return false;
        }



        static void Main(string[] args)
        {
            const string URL_INIT = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr&category=GPU&gpu=RTX%203090,RTX%203080%20Ti,RTX%203080,RTX%203070%20Ti,RTX%203070,RTX%203060%20Ti,RTX%203060&gpu_filter=RTX%203090~12,RTX%203080%20Ti~7,RTX%203080~16,RTX%203070%20Ti~3,RTX%203070~18,RTX%203060%20Ti~8,RTX%203060~2,RTX%202080%20SUPER~1,RTX%202080~0,RTX%202070%20SUPER~0,RTX%202070~0,RTX%202060~6,GTX%201660%20Ti~0,GTX%201660%20SUPER~9,GTX%201660~8,GTX%201650%20Ti~0,GTX%201650%20SUPER~3,GTX%201650~17";
            const int REFRESH = 1000;


            // ---------------Premier lancement de l'application-------------------------------------------------
            string dropFile = CreateDropFile();
            var connectionInit = ConnectionApi(URL_INIT);
            List<GraphicsCard> gpusInit = GenerateGpu(connectionInit);

            Console.WriteLine("Salut c'est Tony. J'ai des contacts dans la Mafia.\n\nQuelle carte graphique recherches tu ? (Entrer le numéro correspondant à la carte graphique souhaitée)");
            List<string> gpusWanted = GetGpuWanted(gpusInit);

            Console.WriteLine();
            Console.WriteLine("Merci, je consulte Laurent");
            // ----------------------Fin Initialisation--------------------------------------------------------------------


            // ---------------Recherche des RTX FE-------------------------------------------------
            string urlBase = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr&category=GPU&gpu=";
            while (true)
            {
                Thread.Sleep(REFRESH);
                Console.Clear();
                DisplayGpuWanted(gpusWanted);

                for (int i = gpusWanted.Count - 1; i >= 0; i--)
                {

                    string urlGpu = urlBase + gpusWanted[i].Replace(" ", "%20");
                    JsonDocument connection = ConnectionApi(urlGpu);

                    var gpus = GenerateGpu(connection);
                    bool gpuDrop = SearchGpu(gpus, dropFile);

                    if (gpuDrop)
                    {
                        gpusWanted.RemoveAt(i);
                    }

                }

                if (File.Exists(dropFile))
                {
                    DisplayOldDrop(dropFile);
                }

                if (gpusWanted.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Votre sélection est vide, un drop a déjà eu lieu pour les références choisies.\nMerci de relancer l'application pour une nouvelle recherche");
                    break;
                }
            }
        }
    }
}
