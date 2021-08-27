using System.Collections.Generic;

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

        public class FeaturedProduct : ProductDetail
        {
        }
    }
}
