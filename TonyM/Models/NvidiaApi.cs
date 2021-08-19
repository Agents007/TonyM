using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TonyM.Models;

namespace TonyM.Modules
{
    class NvidiaApi
    {
        string Url { get; set; }

        public NvidiaApi(string url)
        {
            this.Url = url;
        }


        public async Task<JsonDocument> Connection()
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("user-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 OPR/77.0.4054.277");
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            client.DefaultRequestHeaders.Add("Pragma", "no-cache");

            try
            {
                double timestamp = GlobalMethod.Timestamp();
                string json = await client.GetStringAsync(Url + "&timestamp=" + timestamp);
                var jsonParse = JsonDocument.Parse(json);
                return jsonParse;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return null;
            }
        }
    }
}
