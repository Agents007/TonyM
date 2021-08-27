using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonyM.Models
{
    public class Call
    {
        public bool Success { get; set; }
        public List<ListMap> ListMap { get; set; }

        public bool GetGpu()
        {
            bool inStock = ListMap.First().SearchStock();
            return inStock;
        }
    }
}
