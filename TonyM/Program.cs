using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace TonyM
{

    public class CarteGraphique
    {
        public string displayName { get; set; }
        public string prdStatus { get; set; }
        public string directPurchaseLink { get; set; }
    }



    class Program
    {
        //Récupére les infos via l'API Nvidia
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

        //Désérialisation du Json
        static List<CarteGraphique> GenerateGpu(string json)
        {
            var jsonParse = JObject.Parse(json);

            var result = jsonParse["searchedProducts"]["productDetails"]
                .Where(n => n["isFounderEdition"].Value<bool>() == true)
                .Select(p => new CarteGraphique
                {
                    displayName = (string)p["displayName"],
                    prdStatus = (string)p["prdStatus"],
                    directPurchaseLink = (string)p["retailers"][0]["directPurchaseLink"]
                }).ToList();

            return result;
        }

        //Initialisation
        static void FirstStart()
        {
            Console.WriteLine("Salut c'est Tony, j'ai des contacts dans la Mafia\n");
            Console.WriteLine("Quelle carte graphique recherche tu ?");
            List<string> gpusChoice = new List<string>() { "3060TI", "3070", "3070TI", "3080", "3080TI", "3090" };

            for (int i = 0; i < gpusChoice.Count; i++)
            {
                Console.WriteLine("Choix " + (i + 1) + " : " + gpusChoice[i]);
            }

            List<string> gpus = new List<string>();
            while (true)
            {
                string gpu = Console.ReadLine();
                if (string.IsNullOrEmpty(gpu))
                {
                    break;
                }

                if (gpu == "")
                {
                    Console.WriteLine("Erreur, merci d'entrer la référence d'une carte graphique");
                }
                else if (gpu.Length > 6) || (gpu.Length < é3) {  

                }
            }

            List<string> gpus = new List<string>();
            //GetGpuWanted(gpus);
            //return gpus;

        }

        static void GetGpuWanted(List<string> gpus)
        {

  
            string gpu = Console.ReadLine().Replace(" ", "").ToUpper();
  

            while (true)
            {
                if (!gpu.StartsWith('3'))
                {
                    Console.WriteLine("Nop dsl");
                }
                else
                {
                    gpus.Add(gpu);
                    Console.WriteLine(gpu + " est bien ajouté à la liste");
                }
            }
        }




        static void Main(string[] args)
        {
            const string url = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr&category=GPU&manufacturer=NVIDIA&manufacturer_filter=NVIDIA~1";

            FirstStart();
            string json = getGpuFromNvidia(url);
            var gpus = GenerateGpu(json);

            //List<string> gpusWanted = new List<string>() { "NVIDIA RTX 3070", "NVIDIA RTX 3090" };
            //var gpusFilter = gpus.Where(g => gpusWanted.Any(w => w == g.displayName)).ToList();

            //List<string> gpusWanted = new List<string>();
            //while (true)
            //{
            //    string gpu = Console.ReadLine();
            //    if (string.IsNullOrEmpty(gpu))
            //    {
            //        break;
            //    }

            //    if (gpu == "")
            //    {
            //        Console.WriteLine("Erreur, merci d'entrer la référence d'une carte graphique");
            //    }
            //    else if (gpu.Length > 6) || (gpu.Length < 3) {

            //    }
            //}



            //foreach (var gpu in start)
            //{
            //    Console.WriteLine(gpu);
            //}







            //int nbGpu = 0;
            //foreach (var gpu in gpus)
            //{
            //    nbGpu++;
            //    Console.WriteLine(gpu.displayName + " - " + gpu.prdStatus + " - " + gpu.directPurchaseLink);
            //}

            //Console.WriteLine(nbGpu + " références");


        }
    }
}
