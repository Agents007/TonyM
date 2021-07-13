using System;
using System.Net;

namespace TonyM
{
    class carteGraphique
    {
        string nom { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://api.nvidia.partners/edge/product/search?page=1&limit=9&locale=fr-fr";
            var webClient = new WebClient();

            //webClient.DownloadFile(url, "nvidia.json");
            //Console.WriteLine("OK");

            var json = webClient.DownloadString(url);
            json.
            Console.WriteLine(json);
        }
    }
}
