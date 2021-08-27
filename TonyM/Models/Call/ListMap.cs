using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using TonyM.Modules;

namespace TonyM.Models
{
    public class ListMap
    {
        public string Is_active { get; set; }
        public string Product_url { get; set; }
        public string Price { get; set; }
        public string Fe_sku { get; set; }
        public string Fe_name
        {
            get
            {
                return Fe_sku.Replace("NVGFT", "RTX 3").Replace("_FR", "").Replace("0T", "0 Ti");
            }
        }
        public string Locale { get; set; }

        public bool SearchStock()
        {
            if ((Is_active != "false") && (!String.IsNullOrEmpty(Product_url)))
            {
                OpenBuyPage();
                GlobalMethod.SoundAlert();
                WriteDrop();
                return true;
            }
            else
            {
                Console.WriteLine(Fe_name + " : Pas de stock");
                return false;
            }
        }

        public void OpenBuyPage()
        {
            ProcessStartInfo psi = new()
            {
                UseShellExecute = true,
                FileName = Product_url
            };
            Process.Start(psi);
        }

        public void WriteDrop()
        {
            DateTime date = DateTime.Now;
            CultureInfo cultureFrancais = CultureInfo.GetCultureInfo("fr-FR");

            string dateStr = date.ToString("dd/MM HH:mm:ss", cultureFrancais);
            string drop = dateStr + " : " + Fe_name + " => " + Product_url + "\n";

            if (!Directory.Exists(DropFile.Folder))
            {
                Directory.CreateDirectory(DropFile.Folder);
            }

            while (true)
            {
                try
                {
                    File.AppendAllText(DropFile.PathAndFile, drop);
                    break;
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
