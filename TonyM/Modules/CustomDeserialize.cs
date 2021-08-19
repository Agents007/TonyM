using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TonyM.Models
{
    public static class CustomDeserialize
    {
        public static T ToObject<T>(this JsonElement element, JsonSerializerOptions options = null)
        {
            var bufferWriter = new System.Buffers.ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
                element.WriteTo(writer);
            return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
        }

        public static List<GraphicsCard> DeserializeJson(JsonDocument jsonParse)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            List<GraphicsCard> searchedProducts = jsonParse.RootElement
                .GetProperty("searchedProducts")
                .GetProperty("productDetails")
                .EnumerateArray()
                .Where(n => n.GetProperty("isFounderEdition").GetBoolean())
                .Select(n => n.ToObject<GraphicsCard>(options))
                .ToList();

            GraphicsCard featuredProduct = jsonParse.RootElement
                .GetProperty("searchedProducts")
                .GetProperty("featuredProduct")
                .ToObject<GraphicsCard>(options);


            if ((featuredProduct != null) && (searchedProducts != null))
            {
                searchedProducts.Add(featuredProduct);
                return searchedProducts;
            }
            else if ((featuredProduct != null) && (searchedProducts == null))
            {
                List<GraphicsCard> featuredProductList = new();
                featuredProductList.Add(featuredProduct);
                return featuredProductList;
            }
            else if ((featuredProduct == null) && (searchedProducts != null))
            {
                return searchedProducts;
            }
            return null;
        }
    }


}
