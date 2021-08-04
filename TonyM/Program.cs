using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
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
            string path = "Drop";
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

        static void DisplayGpuWanted(Dictionary<string,string> gpuWanted)
        {
            Console.Write("-- VOTRE SELECTION --\n");
            Console.WriteLine(String.Join(", ", gpuWanted.Keys.OrderBy(o => o)).Replace("NVIDIA RTX ", "")); ;
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
            System.Diagnostics.ProcessStartInfo psi = new()
            {
                UseShellExecute = true,
                FileName = link
            };
            System.Diagnostics.Process.Start(psi);
        }

        static void SoundAlert()
        {
            for (int i = 0; i < 3; i++)
            {
                Console.Beep();
            }
        }

        static double Timestamp()
        {
            DateTime tBase = new(2018, 06, 14); //Champion !!!
            DateTime tNow = DateTime.Now;
            TimeSpan tCal = tNow - tBase;
            double timestamp = Math.Round(tCal.TotalSeconds);
            return timestamp;
        }




        // Constitution de la liste des GPU Visible via l'API, et la liste de sélection utilisateur
        static Dictionary<string, string> GetGpuWanted(List<GraphicsCard> gpusObj, string urlBase)
        {
            List<string> gpusAvailable = gpusObj.Select(g => g.DisplayName).OrderBy(o => o).ToList();
            for (int i = 0; i < gpusAvailable.Count; i++)
            {
                Console.WriteLine("Choix " + (i + 1) + " : " + gpusAvailable[i]);
            }
            Console.WriteLine("Choix 10 : SELECTION TERMINEE\n");

            Dictionary<string, string> gpusUserSelect = new();
            while (true)
            {
                Console.Write("Votre choix : ");
                string choice = Console.ReadLine();
                try
                {
                    int choiceInt = int.Parse(choice);
                    if ((choiceInt > 0) && (choiceInt < (gpusAvailable.Count + 1)))
                    {
                        if (gpusUserSelect.ContainsKey(gpusAvailable[choiceInt - 1]))
                        {
                            Console.WriteLine("Ce GPU fait déjà parti de votre sélection\n");
                        }
                        else
                        {
                            string gpuName = gpusAvailable[choiceInt - 1].Replace("NVIDIA ", "");
                            gpusUserSelect.Add(gpuName, urlBase + gpuName.Replace(" ", "%20"));
                            Console.WriteLine(gpuName + " ajoutée à la liste. Ajouter une autre carte ou terminer la sélection.\n");
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
            using WebClient webClient = new();
            webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            webClient.Headers.Add("Accept", "application/json");
            webClient.Headers.Add("user-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 OPR/77.0.4054.277");
            webClient.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            webClient.Headers.Add("Pragma", "no-cache");
            
            try
            {
                var timestamp = Timestamp();
                string json = webClient.DownloadString(url + "&timestamp=" + timestamp);
                
                //Console.WriteLine(url + "&timestamp=" + timestamp);
                //Reponse du serveur requete HTTP
                //WebHeaderCollection myWebHeaderCollection = webClient.ResponseHeaders;
                //for (int i = 0; i < myWebHeaderCollection.Count; i++)
                //Console.WriteLine("\t" + myWebHeaderCollection.GetKey(i) + " = " + myWebHeaderCollection.Get(i));


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
                List<GraphicsCard> featuredProductList = new();
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
            GraphicsCard gpu = gpus.First();

                string link = gpu.Retailers.Select(g => g.PurchaseLink).ToList().First();

                if ((gpu.PrdStatus != "out_of_stock") && (!String.IsNullOrEmpty(link)))
                {
                    OpenBuyPage(link);
                    SoundAlert();
                    WriteDrop(pathAndFile, gpu.DisplayName, link);
                    return true;
                }
                else
                {
                    Console.WriteLine(gpu.DisplayName + " : " + gpu.PrdStatus);
                }     

            return false;
        }




        static void Main(string[] args)
        {
            const string URL_INIT = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr&category=GPU&gpu=RTX%203090,RTX%203080%20Ti,RTX%203080,RTX%203070%20Ti,RTX%203070,RTX%203060%20Ti,RTX%203060&gpu_filter=RTX%203090~12,RTX%203080%20Ti~7,RTX%203080~16,RTX%203070%20Ti~3,RTX%203070~18,RTX%203060%20Ti~8,RTX%203060~2,RTX%202080%20SUPER~1,RTX%202080~0,RTX%202070%20SUPER~0,RTX%202070~0,RTX%202060~6,GTX%201660%20Ti~0,GTX%201660%20SUPER~9,GTX%201660~8,GTX%201650%20Ti~0,GTX%201650%20SUPER~3,GTX%201650~17";
            string URL_BASE = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr&category=GPU&gpu=";
            const int REFRESH = 1000;


            // ---------------Premier lancement de l'application-------------------------------------------------
            string dropFile = CreateDropFile();
            using JsonDocument connectionInit = ConnectionApi(URL_INIT);
            List<GraphicsCard> gpusInit = GenerateGpu(connectionInit);

            Console.WriteLine("Salut c'est Tony. J'ai des contacts dans la Mafia.\n\nQuelle carte graphique recherches tu ? (Entrer le numéro correspondant à la carte graphique souhaitée)");

            Dictionary<string, string> gpusWanted = GetGpuWanted(gpusInit, URL_BASE);

            Console.WriteLine();
            Console.WriteLine("Merci, je consulte Laurent");
            // ----------------------Fin Initialisation--------------------------------------------------------------------


            // ---------------Recherche des RTX FE-------------------------------------------------       
            while (true)
            {
                Thread.Sleep(REFRESH);
                Console.Clear();
                DisplayGpuWanted(gpusWanted);

                DateTime t1 = DateTime.Now;

                foreach (var gpuW in gpusWanted)
                {
                    using JsonDocument connection = ConnectionApi(gpuW.Value);
                    List<GraphicsCard> gpuGenerated = GenerateGpu(connection);
                    bool gpuDrop = SearchGpu(gpuGenerated, dropFile);

                    if (gpuDrop)
                    {
                        gpusWanted.Remove(gpuW.Key);
                    }

                }


                DateTime t2 = DateTime.Now;
                var diff = t2 - t1;
                Console.WriteLine(diff.TotalMilliseconds);

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
