using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace TonyM
{
    public class CarteGraphique
    {
        public string displayName { get; set; }

        public string prdStatus { get; set; }

        public List<Retailer> retailers { get; set; }
    }

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
        // Récupére les infos via l'API Nvidia
        static string getGpuFromNvidia(string url)
        {
            var webClient = new WebClient();

            string json = null;
            try
            {
                json = webClient.DownloadString(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return getGpuFromNvidia(url); // A corriger pour un retour aux lancement de l'application
            }
            return json;
        }


        // Désérialisation du Json
        static List<ProductDetail> GenerateGpu(string json)
        {
            NvidiaRoot jsonObj = JsonSerializer.Deserialize<NvidiaRoot>(json);

            var gpuList = jsonObj.searchedProducts.productDetails
                .Where(n => n.isFounderEdition)
                .ToList();

            var featuredProduct = jsonObj.searchedProducts.featuredProduct;

            gpuList.Add(featuredProduct);

            return gpuList;
        }

        // Constitution de la liste des GPU Visible via l'API
        static List<string> GetGpuVisible(List<ProductDetail> gpusApi) 
        {
            List<string> gpus = new List<string>();
            foreach (var gpuApi in gpusApi)
            {
                gpus.Add(gpuApi.displayName);
            }
            return gpus;
        }



        //Constitution de la liste des GPU Visible via l'API
        static List<string> GetGpuWanted(List<string> gpusVisible)
        {
            List<string> gpus = new List<string>();


            for (int i = 0; i < gpusVisible.Count; i++)
            {
                Console.WriteLine("Choix " + (i + 1) + " : " + gpusVisible[i]);
            }
            Console.WriteLine("Choix 10 : SELECTION TERMINEE\n");


            while (true)
            {
                Console.Write("Votre choix : ");
                string choice = Console.ReadLine();
                try
                {
                    int choiceInt = int.Parse(choice);
                    if ((choiceInt > 0) && (choiceInt < (gpusVisible.Count + 1)))
                    {
                        if (gpus.Contains(gpusVisible[choiceInt - 1]))
                        {
                            Console.WriteLine("Ce GPU fait déjà partis de votre sélection\n");
                        } 
                        else
                        {
                            gpus.Add(gpusVisible[choiceInt - 1]);
                            Console.WriteLine(gpusVisible[choiceInt - 1] + " ajoutée à la liste. Ajoutez une autre carte ou terminé la sélection.\n");
                        }
                    }
                    else if ((choiceInt  == 10) && (gpus.Count > 0))
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

            return gpus;
        }

        static void OpenBuyPage(string link)
        {
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = link;
            System.Diagnostics.Process.Start(psi);
        }

        static void DisplayGpuWanted(List<string> gpuWanted)
        {
            Console.Write("VOTRE SELECTION -> ");
            Console.WriteLine(String.Join(", ", gpuWanted));
        }

        static void SearchGpu(int refresh, List<ProductDetail> gpus, List<string> gpusWanted)
        {
            while (true)
            {
                Thread.Sleep(refresh);
                Console.Clear();
                DisplayGpuWanted(gpusWanted);

                var gpusFilter = gpus.Where(g => gpusWanted.Any(w => w == g.displayName)).ToList();

                foreach (var gpu in gpusFilter)
                {
                    if (gpu.prdStatus != "out_of_stock")
                    {
                        string link = "";
                        foreach (var item in gpu.retailers)
                        {
                            link = item.directPurchaseLink;
                        }

                        OpenBuyPage(link);
                        gpusWanted.Remove(gpu.displayName);
                    }
                    else
                    {
                        Console.WriteLine(gpu.displayName + " : " + gpu.prdStatus);
                    }
                }
            }
        }





        static void Main(string[] args)
        {
            const string url = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr&category=GPU&gpu=RTX%203090,RTX%203080%20Ti,RTX%203080,RTX%203070%20Ti,RTX%203070,RTX%203060%20Ti,RTX%203060&gpu_filter=RTX%203090~12,RTX%203080%20Ti~7,RTX%203080~16,RTX%203070%20Ti~3,RTX%203070~18,RTX%203060%20Ti~8,RTX%203060~2,RTX%202080%20SUPER~1,RTX%202080~0,RTX%202070%20SUPER~0,RTX%202070~0,RTX%202060~6,GTX%201660%20Ti~0,GTX%201660%20SUPER~9,GTX%201660~8,GTX%201650%20Ti~0,GTX%201650%20SUPER~3,GTX%201650~17";
            const int refresh = 3000;

            string json = getGpuFromNvidia(url);

            DateTime t1 = DateTime.Now;



            var listGpu = new List<CarteGraphique>();

            using var jsonParse = JsonDocument.Parse(json); // Be sure to dispose the JsonDocument!

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var products = jsonParse.RootElement
                .GetProperty("searchedProducts") // Get the searchedProducts value
                .GetProperty("productDetails")   // Get the productDetails value
                .EnumerateArray()                // Enumerate its items
                .Where(n => n.GetProperty("isFounderEdition").GetBoolean()) // Filter on those for which isFounderEdition == true
                .Select(n => n.ToObject<CarteGraphique>(options)) // Deserialize to a CarteGraphique
                .ToList();

            // Add the searchedProducts.featuredProduct item to the list.
            var featuredProduct = jsonParse.RootElement
                .GetProperty("searchedProducts")
                .GetProperty("featuredProduct")
                .ToObject<CarteGraphique>(options);

            products.Add(featuredProduct);








            foreach (var item in products)
            {
                Console.WriteLine(item.displayName);
            }


            //listGpu.Add(jsonObj);



            //foreach (var item in listGpu)
            //{
            //    Console.WriteLine(item.displayName);
            //    Console.WriteLine(item.PrdStatus);
            //    foreach (var it in item.retailers)
            //    {
            //        Console.WriteLine(it.directPurchaseLink);
            //    }
            //}




            DateTime t2 = DateTime.Now;
            var diff = (int)((t2 - t1).TotalMilliseconds);
            Console.WriteLine("durée " + diff);


            //var gpus = GenerateGpu(json);

            //Console.WriteLine("Salut c'est Tony. J'ai des contacts dans la Mafia.\n\nQuelle carte graphique recherches tu ?");

            //var gpusVisible = GetGpuVisible(gpus);
            //var gpusWanted = GetGpuWanted(gpusVisible);

            //Console.WriteLine();
            //Console.WriteLine("Merci, je consulte Laurent");

            //SearchGpu(refresh, gpus, gpusWanted);

        }
    }
}
