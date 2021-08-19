using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using TonyM.Modules;

namespace TonyM.Models
{
    public class GraphicsCard
    {
        public string DisplayName { get; set; }
        public string PrdStatus { get; set; }
        public List<Retailer> Retailers { get; set; }
        public DateTime LastAvailability { get; set; }
        public bool Wanted { get; set; }
        public string ApiLink { get; init; }


        public void OpenBuyPage()
        {
            string link = GetLink();

            ProcessStartInfo psi = new()
            {
                UseShellExecute = true,
                FileName = link
            };
            Process.Start(psi);
        }

        public void WriteDrop(string pathAndFile)
        {
            DateTime date = DateTime.Now;
            CultureInfo cultureFrancais = CultureInfo.GetCultureInfo("fr-FR");

            string link = GetLink();

            string dateStr = date.ToString("dd/MM HH:mm:ss", cultureFrancais);
            string drop = dateStr + " : " + DisplayName.Replace("NVIDIA ", "") + " -> " + link + "\n";

            while (true)
            {
                try
                {
                    File.AppendAllText(pathAndFile, drop);
                    break;
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }

        public string GetLink()
        {
            string link = Retailers.Select(g => g.PurchaseLink).ToList().First();
            return link;
        }

        public string NameForUrl()
        {
            string customName = DisplayName.Replace("NVIDIA ", "").Replace(" ", "%20");
            return customName;
        }

        public bool SearchStock(string pathAndFile)
        {
            string link = GetLink();

            if ((PrdStatus != "out_of_stock") && (!String.IsNullOrEmpty(link)))
            {
                OpenBuyPage();
                GlobalMethod.SoundAlert();                
                WriteDrop(pathAndFile);
                return true;
            }
            else
            {
                Console.WriteLine(DisplayName + " : " + PrdStatus);
                return false;
            }
        }

    }  
}
