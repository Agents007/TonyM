using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using TonyM.Modules;

namespace TonyM.Models
{
    public class ListMap
    {
        public string is_active { get; set; }
        public string product_url { get; set; }
        public string price { get; set; }
        public string fe_sku { get; set; }
        public string locale { get; set; }

        public bool SearchStock()
        {
            if ((is_active != "false") && (!String.IsNullOrEmpty(product_url)))
            {
                OpenBuyPage();
                GlobalMethod.SoundAlert();
                WriteDrop();
                return true;
            }
            else
            {
                Console.WriteLine(fe_sku + " : Pas de stock");
                return false;
            }
        }

        public void OpenBuyPage()
        {
            ProcessStartInfo psi = new()
            {
                UseShellExecute = true,
                FileName = product_url
            };
            Process.Start(psi);
        }

        public void WriteDrop()
        {
            DateTime date = DateTime.Now;
            CultureInfo cultureFrancais = CultureInfo.GetCultureInfo("fr-FR");

            string dateStr = date.ToString("dd/MM HH:mm:ss", cultureFrancais);
            string drop = dateStr + " : " + fe_sku + product_url + "\n";

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
