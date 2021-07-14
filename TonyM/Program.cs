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

        // Constitution de la liste des GPU Visible via l'API
        static List<string> GetGpuVisible(List<CarteGraphique> gpusApi) 
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
                            Console.WriteLine(gpusVisible[choiceInt - 1] + " ajoutée à la liste, voulez vous ajouter une autre carte ?\n");
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


            Console.WriteLine("\nVotre sélection est la suivante :");
            foreach (var gpu in gpus)
            {
                Console.WriteLine("- " + gpu);
            }
            Console.WriteLine("\nSi un gpu est disponible, Tony ouvrira la page web correspondante");


            return gpus;
        }





        static void Main(string[] args)
        {
            const string url = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr&category=GPU&manufacturer=NVIDIA&manufacturer_filter=NVIDIA~1";


            string json = getGpuFromNvidia(url);
            var gpus = GenerateGpu(json);

            Console.WriteLine("Salut c'est Tony, j'ai des contacts dans la Mafia\n\nQuelle carte graphique recherche tu ?");

            var gpusVisible = GetGpuVisible(gpus);

            var gpusWanted = GetGpuWanted(gpusVisible);

            var gpusFilter = gpus.Where(g => gpusWanted.Any(w => w == g.displayName)).ToList();


            var uri = "https://www.google.com";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);




            foreach (var gpu in gpusFilter)
            {
                Console.WriteLine(gpu.displayName + " - " + gpu.prdStatus + " - " + gpu.directPurchaseLink);
            }


        }
    }
}
