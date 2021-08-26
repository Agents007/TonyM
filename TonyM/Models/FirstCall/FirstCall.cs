using System.Collections.Generic;
using System.Linq;

namespace TonyM.Models
{
    public class FirstCall
    {
        public SearchedProducts SearchedProducts { get; set; }

        public List<ProductDetail> GetAllFe()
        {
            List<ProductDetail> gpusAvailable = SearchedProducts.ProductDetails;
            gpusAvailable.Add(SearchedProducts.FeaturedProduct);
            gpusAvailable = gpusAvailable.Where(g => g.IsFounderEdition).ToList();
            return gpusAvailable;
        }

        //public bool GetGpu()
        //{
        //    var fProduct = SearchedProducts.FeaturedProduct;
        //    var dProduct = SearchedProducts.ProductDetails.Where(g => g.IsFounderEdition).ToList();

        //    if ((fProduct is not null) && (fProduct.IsFounderEdition))
        //    {
        //        bool inStock = fProduct.SearchStock();
        //        return inStock;
        //    }
        //    else
        //    {
        //        bool inStock = dProduct.First().SearchStock();
        //        return inStock;
        //    }
        //}
    }
}
