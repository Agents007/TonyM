using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TonyM.Modules
{
    public static class NvidiaApi
    {
        public static async Task<string> Connection(string Url)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("user-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 OPR/77.0.4054.277");
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            client.DefaultRequestHeaders.Add("Pragma", "no-cache");

            string json = null;
            try
            {
                double timestamp = GlobalMethod.Timestamp();
                json = await client.GetStringAsync(Url + "&timestamp=" + timestamp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
            return json;
        }
    }
}
