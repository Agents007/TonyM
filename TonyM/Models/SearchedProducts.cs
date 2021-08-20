using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TonyM.Models.ProductDetail;

namespace TonyM.Models
{
    public class SearchedProducts
    {
        public FeaturedProduct FeaturedProduct { get; set; }
        public List<ProductDetail> ProductDetails { get; set; }
    }
}
