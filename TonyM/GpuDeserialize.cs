using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonyM
{
    public class GraphicsCard
    {
        public string DisplayName { get; set; }
        public string PrdStatus { get; set; }
        public List<Retailer> Retailers { get; set; }
        public DateTime LastAvailability { get; set; }
    }  

    public class Retailer
    {
        public string SalePrice { get; set; }
        public string DirectPurchaseLink { get; set; }
        public string PurchaseLink { get; set; }
    }
}
