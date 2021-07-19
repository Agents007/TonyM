using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonyM
{
    public class NvidiaRoot
    {
        public object categories { get; set; }
        public List<Filter> filters { get; set; }
        public object filterGroups { get; set; }
        public object search { get; set; }
        public string version { get; set; }
        public List<Sort> sort { get; set; }
        public Pagination pagination { get; set; }
        public SearchedProducts searchedProducts { get; set; }
        public Disclaimer disclaimer { get; set; }
    }

    public class FilterValue
    {
        public string dispValue { get; set; }
        public object dispValueDesription { get; set; }
        public object groupType { get; set; }
        public int units { get; set; }
        public bool @checked { get; set; }
        public object imageURL { get; set; }
        public bool isValidate { get; set; }
    }

    public class Filter
    {
        public string displayName { get; set; }
        public string filterField { get; set; }
        public string displayMaxValues { get; set; }
        public string fieldType { get; set; }
        public int? selectedMinRangeVal { get; set; }
        public int? selectedMaxRangeVal { get; set; }
        public int? defaultMinRangeVal { get; set; }
        public int? defaultMaxRangeVal { get; set; }
        public string unitsOfMeasure { get; set; }
        public bool @checked { get; set; }
        public object units { get; set; }
        public List<FilterValue> filterValues { get; set; }
        public string dataType { get; set; }
        public bool showCount { get; set; }
        public int filterDisplayOrder { get; set; }
    }

    public class Sort
    {
        public string displayName { get; set; }
        public string value { get; set; }
        public bool selected { get; set; }
    }

    public class Pagination
    {
        public int page { get; set; }
        public int limit { get; set; }
        public int totalRecords { get; set; }
        public bool featuredProductIncludedInCount { get; set; }
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

    public class ProductInfo
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class CompareProductInfo
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class ProductDetail {
        public string displayName { get; set; }
        public int totalCount { get; set; }
        public int productID { get; set; }
        public string imageURL { get; set; }
        public string productTitle { get; set; }
        public string digitialRiverID { get; set; }
        public string productSKU { get; set; }
        public string productUPC { get; set; }
        public string productUPCOriginal { get; set; }
        public string productPrice { get; set; }
        public bool productAvailable { get; set; }

        public object customerReviewCount { get; set; }
        public bool isFounderEdition { get; set; }
        public bool isFeaturedProduct { get; set; }
        public bool certified { get; set; }
        public string manufacturer { get; set; }
        public string locale { get; set; }
        public bool isFeaturedProdcutFoundInSecondSearch { get; set; }
        public string category { get; set; }
        public string gpu { get; set; }
        public string purchaseOption { get; set; }
        public string prdStatus { get; set; }
        public object minShipDays { get; set; }
        public object maxShipDays { get; set; }
        public object shipInfo { get; set; }
        public bool isOffer { get; set; }
        public string offerText { get; set; }
        public List<Retailer> retailers { get; set; }
        public List<ProductInfo> productInfo { get; set; }
        public List<CompareProductInfo> compareProductInfo { get; set; }
    }


    public class FeaturedProduct : ProductDetail
    {

    }

    public class SearchedProducts
    {
        public int totalProducts { get; set; }
        public bool featuredProductIncludedInCount { get; set; }
        public bool featuredProductsFlag { get; set; }
        public FeaturedProduct featuredProduct { get; set; }
        public List<ProductDetail> productDetails { get; set; }
        public List<object> suggestedProductDetails { get; set; }
    }

    public class Disclaimer
    {
        public string text { get; set; }
    }

}
