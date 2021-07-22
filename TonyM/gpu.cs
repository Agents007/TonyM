using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonyM
{

    public class GraphicsCard
    {
        public string displayName { get; set; }
        public string prdStatus { get; set; }
        public List<Retailer> retailers { get; set; }
    }

   

    public class Retailer
    {
        public int productId { get; set; }
        public string productTitle { get; set; }
        public string logoUrl { get; set; }
        public bool isAvailable { get; set; }
        public string salePrice { get; set; }
        public string directPurchaseLink { get; set; }
        public string purchaseLink { get; set; }
        public bool hasOffer { get; set; }
        public object offerText { get; set; }
        public string partnerId { get; set; }
        public string storeId { get; set; }
        public string upc { get; set; }
        public string sku { get; set; }
        public int stock { get; set; }
        public string retailerName { get; set; }
        public int type { get; set; }
    }

    

}
