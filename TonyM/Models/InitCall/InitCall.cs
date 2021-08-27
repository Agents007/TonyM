using System.Collections.Generic;
using System.Linq;

namespace TonyM.Models
{
    public class InitCall
    {
        public SearchedProducts SearchedProducts { get; set; }

        public List<ProductDetail> GetAllFe()
        {
            List<ProductDetail> gpusAvailable = SearchedProducts.ProductDetails;
            gpusAvailable.Add(SearchedProducts.FeaturedProduct);
            gpusAvailable = gpusAvailable.Where(g => g.IsFounderEdition).ToList();
            return gpusAvailable;
        }
    }
}
