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
        public DateTime lastAvailability { get; set; }
    }  

    public class Retailer
    {
        public string salePrice { get; set; }
        public string directPurchaseLink { get; set; }
        public string purchaseLink { get; set; }
    }
    

}
