using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonyM.Models
{
    public class Retailer
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string LogoUrl { get; set; }
        public bool IsAvailable { get; set; }
        public string SalePrice { get; set; }
        public string DirectPurchaseLink { get; set; }
        public string PurchaseLink { get; set; }
        public bool HasOffer { get; set; }
        public object OfferText { get; set; }
        public string PartnerId { get; set; }
        public string StoreId { get; set; }
        public string Upc { get; set; }
        public string Sku { get; set; }
        public int Stock { get; set; }
        public string RetailerName { get; set; }
        public int Type { get; set; }
    }
}
