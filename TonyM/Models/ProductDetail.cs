using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TonyM.Modules;

namespace TonyM.Models
{
    public class ProductDetail
    {
        public string DisplayName { get; set; }
        public int TotalCount { get; set; }
        public int ProductID { get; set; }
        public string ImageURL { get; set; }
        public string ProductTitle { get; set; }
        public string DigitialRiverID { get; set; }
        public string ProductSKU { get; set; }
        public string ProductUPC { get; set; }
        public string ProductUPCOriginal { get; set; }
        public string ProductPrice { get; set; }
        public bool ProductAvailable { get; set; }
        public object ProductRating { get; set; }
        public object CustomerReviewCount { get; set; }
        public bool IsFounderEdition { get; set; }
        public bool IsFeaturedProduct { get; set; }
        public bool Certified { get; set; }
        public string Manufacturer { get; set; }
        public string Locale { get; set; }
        public bool IsFeaturedProdcutFoundInSecondSearch { get; set; }
        public string Category { get; set; }
        public string Gpu { get; set; }
        public string PurchaseOption { get; set; }
        public string PrdStatus { get; set; }
        public object MinShipDays { get; set; }
        public object MaxShipDays { get; set; }
        public object ShipInfo { get; set; }
        public bool IsOffer { get; set; }
        public string OfferText { get; set; }
        public bool UserWanted { get; set; }
        public List<Retailer> Retailers { get; set; }
        public List<ProductInfo> ProductInfo { get; set; }
        public List<CompareProductInfo> CompareProductInfo { get; set; }

        public string NameForUrl()
        {
            string customName = DisplayName.Replace("NVIDIA ", "").Replace(" ", "%20");
            return customName;
        }

        public string GetLink()
        {
            string link = Retailers.Select(g => g.PurchaseLink).ToList().First();
            return link;
        }

        public static void OpenBuyPage(string link)
        {
            ProcessStartInfo psi = new()
            {
                UseShellExecute = true,
                FileName = link
            };
            Process.Start(psi);
        }

        public void WriteDrop(string link)
        {
            DateTime date = DateTime.Now;
            CultureInfo cultureFrancais = CultureInfo.GetCultureInfo("fr-FR");

            string pathAndFile = GlobalMethod.GetDropFile();

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

        public bool SearchStock()
        {
            string link = GetLink();

            if ((PrdStatus != "out_of_stock") && (!String.IsNullOrEmpty(link)))
            {
                OpenBuyPage(link);
                GlobalMethod.SoundAlert();
                WriteDrop(link);
                return true;
            }
            else
            {
                Console.WriteLine(DisplayName + " : Pas de stock");
                return false;
            }
        }


        public class FeaturedProduct : ProductDetail
        {
        }
    }
}
