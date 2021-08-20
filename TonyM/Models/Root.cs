using System.Linq;

namespace TonyM.Models
{
    public class Root
    {
        public SearchedProducts SearchedProducts { get; set; }

        public bool GetGpu()
        {
            var fProduct = SearchedProducts.FeaturedProduct;
            var dProduct = SearchedProducts.ProductDetails.Where(g => g.IsFounderEdition).ToList();

            if ((fProduct is not null) && (fProduct.IsFounderEdition))
            {
                bool inStock = fProduct.SearchStock();
                return inStock;
            }
            else
            {
                bool inStock = dProduct.First().SearchStock();
                return inStock;
            }
        }
    }
}
